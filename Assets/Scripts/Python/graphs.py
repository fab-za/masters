import numpy as np
from numpy.core.numeric import NaN
from numpy.lib.twodim_base import tri
import pandas as pd
import matplotlib.pyplot as plt
# import scipy.optimize.curve_fit as cf
import os
import math
import pingouin as pg
import scipy.stats as stats

#----------- INITIALISE GLOBAL VARIABLES

curDir = os.path.dirname(os.path.realpath(__file__))
dataDir = curDir.replace("Scripts\Python","Data")
numParticipants = 1

comparisonDir = "Pairings"
comparisonInds = range(6)
comparisonFile = "comparison_result.csv"
comparisonCols = ["Experiment Index", "Participant Index", "Trial Left Frequency", "Trial Left Roughness", "Trial Left Amplitude", "Trial Right Frequency", "Trial Right Roughness", "Trial Right Amplitude", "Left Frequency", "Left Roughness", "Left Amplitude", "Right Frequency", "Right Roughness", "Right Amplitude", "Visual Frequency Difference Between Sides", "Haptic Frequency Difference Between Sides", "Amplitude Difference Between Sides"]
comparisonPatterns = ["20 Hz Tensioned","20 Hz Control","110 Hz Tensioned","110 Hz Control","200 Hz Tensioned","200 Hz Control"]


#---------- READING FUNCTIONS
def readCSV(targetDir, targetFile, indArray, columnArray):
    compiledFileDict = dict.fromkeys(indArray)
    for i in indArray:
        fileDir = os.path.join(dataDir,targetDir)
        fileDir = os.path.join(fileDir, str(i))
        fileDir = os.path.join(fileDir, targetFile)

        currentFileDict = dict.fromkeys(columnArray)
        for c in columnArray:
            currentFileDict[c] = pd.read_csv(fileDir, skipinitialspace=True,index_col=False, usecols=[c], delimiter=",").T

        compiledFileDict[i] = currentFileDict
    
    return compiledFileDict

#--------- READ FILES
comparisonDict = readCSV(comparisonDir, comparisonFile, comparisonInds, comparisonCols) 

#--------- PLOT PARAMETERS
def definePlots():
    pltParameters = {
        "hapticFrequencyComparison_box": {
            "data": comparisonDict,
            "targetdf": 0,
            "indArray": [0,3,1,4,2,5],
            "targetVariable": "Haptic Frequency Difference Between Sides",
            "suptitle": "Perceived Haptic Frequency Difference Between Sides",
            "subplt_titles": [""],
            "plt_num": 1,
            "xlabel": "Pattern",
            "ylabel": "Frequency (Hz)",
            "xmin": 0,
            "xmax": 10,
            "ymin": 0,
            "ymax": 160,
            "xNames": comparisonPatterns
        },
        "hapticFrequencyComparison_dist": {
            "data": comparisonDict,
            "targetdf": 0,
            "indArray": [0,3,1,4,2,5],
            "targetVariable": "Haptic Frequency Difference Between Sides",
            "suptitle": "Distribution",
            "subplt_titles": comparisonPatterns,
            "plt_num": 6,
            "xlabel": "Frequency (Hz)",
            "ylabel": "Distribution",
            "xmin": 0,
            "xmax": 4,
            "ymin": 0,
            "ymax": 0.3,
            "xNames": ["20Hz", "110Hz", "200Hz"]
        },
    }
    return pltParameters

# ------------ PLOTTING FUNCTIONS
def initEmptyDataFrame(numdatapoints,cols):
    emptarray = np.empty((numdatapoints,len(cols)))
    df = pd.DataFrame(emptarray, columns=cols)
    return df

def createTargetArray(cur):
    # tempArray = []
    numDataPoints = len(cur["data"][0][cur["targetVariable"]].loc[[cur["targetVariable"]]].T)
    tempDataframe = initEmptyDataFrame(numDataPoints, cur["indArray"])

    for i in cur["indArray"]:
        tempData = cur["data"][i][cur["targetVariable"]].loc[[cur["targetVariable"]]].T
        # print(tempData)
        # tempArray = [tempArray, tempData]
        tempDataframe[i] = tempData
    
    return tempDataframe

def plotBox(cur):
    fig, axs = plt.subplots(ncols=cur["plt_num"], squeeze=False, figsize=(8,8))
    fig.suptitle(cur["suptitle"])

    for subplot in range(cur["plt_num"]):
        # b = time_all[experiments[i]].plot(kind="box", ax=axs[i])
        a,bp = cur["targetdf"].boxplot(ax=axs[0,subplot], return_type="both", showmeans=True)

        # m = [round(item.get_ydata()[0], 1) for item in bp['means']]

        # axs[i].set_title(cur["subplt_titles"][i])
        axs[0,subplot].set_xlabel(cur["xlabel"])
        axs[0,0].set_ylabel(cur["ylabel"])
        axs[0,subplot].set_xticklabels(labels=cur["xNames"], rotation=45, fontsize=8, ha="right")

        axs[0,subplot].set_xlim(cur["xmin"], cur["xmax"])
        axs[0,subplot].set_ylim(cur["ymin"], cur["ymax"])

def checkDistribution(cur):
    figD, axsD = plt.subplots(ncols=cur["plt_num"], squeeze=False, figsize=(8,8))
    figD.suptitle(cur["suptitle"])

    for subplot in range(cur["plt_num"]):
        w, pvalue = stats.shapiro(cur["targetdf"][subplot])
        print(w, pvalue)

        axsD[0,subplot].hist(cur["targetdf"][subplot], bins=10, histtype='bar', ec='k') 
        axsD[0,subplot].set_title(cur["subplt_titles"][subplot])
        axsD[0,subplot].set_xlabel(cur["xlabel"])
        axsD[0,0].set_ylabel(cur["ylabel"])

def findWilcoxonSignRank(cur):
    for subplot in range((int)(cur["plt_num"]/2)):
        w, pvalue = stats.wilcoxon(cur["targetdf"][subplot], cur["targetdf"][subplot+3], alternative="greater")
        print(cur["xNames"][subplot], w, pvalue)
    
#----------- CALL PLOT FUNCTIONS
plt.close('all')
plotParameters = definePlots()

plotParameters["hapticFrequencyComparison_box"]["targetdf"] = createTargetArray(plotParameters["hapticFrequencyComparison_box"])
plotParameters["hapticFrequencyComparison_dist"]["targetdf"] = createTargetArray(plotParameters["hapticFrequencyComparison_dist"])

# PLOT INDV RESULT
# plotParameters["hapticFrequencyComparison_box"]["targetdf"] = pd.DataFrame(plotParameters["hapticFrequencyComparison_box"]["targetdf"].iloc[14]).T

# print(type(targetdf))

#--- PLOT BOX
plotBox(plotParameters["hapticFrequencyComparison_box"])

# could create an analysis for percentage of people who thought X was rougher than Y
# ie. do a check between left and right results for each pair

#--- CHECK DISTRIBUTION (PLOT HISTOGRAMS)
checkDistribution(plotParameters["hapticFrequencyComparison_dist"])

#--- RUN WILCOXON SIGN RANK TEST
findWilcoxonSignRank(plotParameters["hapticFrequencyComparison_dist"])

#------------- SHOW PLOTS
plt.show()

print("done")
