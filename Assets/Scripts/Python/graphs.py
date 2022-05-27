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
comparisonPatterns = ["80 Hz Tensioned","110 Hz Tensioned","200 Hz Tensioned","80 Hz Control","110 Hz Control","200 Hz Control"]


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
            "indArray": comparisonInds,
            "targetVariable": "Haptic Frequency Difference Between Sides",
            "suptitle": "Perceived Haptic Frequency Difference Between Sides",
            "subplt_titles": [""],
            "plt_num": 1,
            "xlabel": "Pattern",
            "ylabel": "Frequency (Hz)",
            "xmin": 0,
            "xmax": 10,
            "ymin": 0,
            "ymax": 120,
            "xNames": comparisonPatterns
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
        print(tempData)
        # tempArray = [tempArray, tempData]
        tempDataframe[i] = tempData
    
    return tempDataframe

def plotBox(cur):
    fig, axs = plt.subplots(ncols=cur["plt_num"], squeeze=False, figsize=(8,8))
    fig.suptitle(cur["suptitle"])

    for subplot in range(cur["plt_num"]):
        targetdf = createTargetArray(cur)
        print(type(targetdf))

        # b = time_all[experiments[i]].plot(kind="box", ax=axs[i])
        a,bp = targetdf.boxplot(ax=axs[0,subplot], return_type="both", showmeans=True)

        # m = [round(item.get_ydata()[0], 1) for item in bp['means']]

        # axs[i].set_title(cur["subplt_titles"][i])
        axs[0,subplot].set_xlabel(cur["xlabel"])
        axs[0,0].set_ylabel(cur["ylabel"])
        axs[0,subplot].set_xticklabels(labels=cur["xNames"], rotation=45, fontsize=8, ha="right")

        axs[0,subplot].set_xlim(cur["xmin"], cur["xmax"])
        axs[0,subplot].set_ylim(cur["ymin"], cur["ymax"])
            
    
#----------- CALL PLOT FUNCTIONS
plotParameters = definePlots()
plotBox(plotParameters["hapticFrequencyComparison_box"])

#------------- SHOW PLOTS
plt.show()

print("done")
