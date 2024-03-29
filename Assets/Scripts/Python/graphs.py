from re import I
from click import MultiCommand
import numpy as np
from numpy.core.numeric import NaN
from numpy.lib.twodim_base import tri
import pandas as pd
import matplotlib.pyplot as plt
import os
import math
import pingouin as pg
import scipy.stats as stats
import scipy as sy
from scipy.optimize import curve_fit as cf

#----------- INITIALISE GLOBAL VARIABLES

curDir = os.path.dirname(os.path.realpath(__file__))
dataDir = curDir.replace("Scripts\Python","Data")

comparisonDir = "Pairings"
comparisonInds = range(6)
comparisonInds_reordered = [0,3,1,4,2,5]
comparisonFile = "comparison_result.csv"
comparisonCols = ["Experiment Index", "Participant Index", "Trial Left Frequency", "Trial Left Roughness", "Trial Left Amplitude", "Trial Right Frequency", "Trial Right Roughness", "Trial Right Amplitude", "Left Frequency", "Left Roughness", "Left Amplitude", "Right Frequency", "Right Roughness", "Right Amplitude", "Visual Frequency Difference Between Sides", "Haptic Frequency Difference Between Sides", "Amplitude Difference Between Sides"]
comparisonPatterns = ["20 Hz Tensioned","20 Hz Control","110 Hz Tensioned","110 Hz Control","200 Hz Tensioned","200 Hz Control"]
comparisonPatternsInt = {0: 20, 1: 20, 2:110, 3:110, 4:200, 5:200}
# [20,20,110,110,200,200]

multimodalDir = "Multimodal\Pairings"
multimodalInds = ["00", "01", "10", "11", "20", "21", "30", "31"]
multimodalInds_rough = ["00","10","20","30"]
multimodalInds_smooth = ["01","11","21","31"]
multimodalFile = "multimodal_result.csv"
multimodalCols = ["Phase", "Experiment Index", "Participant Index", "Trial Visual Frequency", "Trial Haptic Frequency", "Trial Amplitude", "Participant Class", "Response Time", "Class Error"]
multimodalTitles = ["Unimodal Visual", "Unimodal Haptic", "Multimodal", "Multimodal + Tension"]
multimodalPatterns = ["Rougher", "Smoother"]

JNDDir = "JND\Pairings"
JNDVisualInds = ["0_0", "0_1", "0_2", "0_3", "0_4", "0_5", "0_6","0_7","0_8","0_9","0_10","0_11","0_12","0_13","0_14","0_15","0_16","0_17","0_18","0_19"]
JNDHapticInds = ["1_0", "1_1", "1_2", "1_3", "1_4", "1_5", "1_6","1_7","1_8","1_9","1_10","1_11","1_12","1_13","1_14","1_15","1_16","1_17","1_18","1_19"]
JNDMulti0Inds = ["2_9","2_10","2_11","2_12","2_13","2_14","2_15","2_16","2_17","2_18","2_19"]
JNDMulti16Inds = ["5_9","5_10","5_11","5_12","5_13","5_14","5_15","5_16","5_17","5_18","5_19"]
JNDMulti33Inds = ["6_9","6_10","6_11","6_12","6_13","6_14","6_15","6_16","6_17","6_18","6_19"]
JNDMulti50Inds = ["3_9","3_10","3_11","3_12","3_13","3_14","3_15","3_16","3_17","3_18","3_19"]
JNDMulti100Inds = ["4_9","4_10","4_11","4_12","4_13","4_14","4_15","4_16","4_17","4_18","4_19"]
JNDInds = JNDVisualInds + JNDHapticInds + JNDMulti0Inds + JNDMulti16Inds + JNDMulti33Inds + JNDMulti50Inds + JNDMulti100Inds

JNDFile = "JND_result.csv"
JNDCols = ["Phase", "Experiment Index", "Participant Index", "Trial Visual Frequency", "Trial Haptic Index", "Trial Haptic Frequency", "Percentage Perceived Smoother"]
JNDTitles = ["0% Noise ", "16% Noise", "33% Noise", "50% Noise", "100% Noise"]
# JNDPatterns = ["Rougher", "Smoother"]
JNDVisualBaseline = 35
JNDHapticBaseline = 20
JNDMultiVisualBaseline = 38
JNDMultiVisualEqBaseline = 20+4*(JNDMultiVisualBaseline-JNDVisualBaseline)
JNDMultiHapticBaseline = 20

#---------- READING FUNCTIONS
def readCSV(targetDir, targetFile, indArray, columnArray):
    compiledFileDict = dict.fromkeys(indArray)
    for i in indArray:
        fileDir = os.path.join(dataDir,targetDir)
        fileDir = os.path.join(fileDir, str(i))
        fileDir = os.path.join(fileDir, targetFile)

        if(os.path.exists(fileDir)):
            currentFileDict = dict.fromkeys(columnArray)

            for c in columnArray:
                currentFileDict[c] = pd.read_csv(fileDir, skipinitialspace=True,index_col=False, usecols=[c], delimiter=",").T

            compiledFileDict[i] = currentFileDict
        else:
            print("file did not exist: ", fileDir)
    
    return compiledFileDict

def initEmptyDataFrame(numdatapoints,cols):
    emptarray = np.empty((numdatapoints,len(cols)))
    df = pd.DataFrame(emptarray, columns=cols)
    return df

def computeAccuracyPerMode(start, data):
    correctCount = 0
    for i in range(3):
        cur = start+i
        if(data[cur].item() == 0):
            correctCount += 1
    
    accuracy = (correctCount/3)*100

    return accuracy

def compileAccuracies():
    accuraciesdf = initEmptyDataFrame(numMultimodalPoints,multimodalInds)
    accuraciesdf.columns = multimodalInds
    for n in range(numMultimodalPoints):
        for pattern in multimodalInds:
            data = multimodalDict[pattern]["Class Error"]
            start = n*3
            acc = computeAccuracyPerMode(start, data)
            accuraciesdf.loc[n, pattern] = acc

    multimodalDict["Accuracies"] = accuraciesdf

def reject_outliers(data, m):
    return data[abs(data - np.mean(data)) < m * np.std(data)]

def compileJND(phaseInds, targetDict, xVariable, baseline):
    JNDdf = initEmptyDataFrame(len(phaseInds),["empty"])
    xFrequencies = []
    JNDdf.index = phaseInds

    for i in phaseInds:
        numJNDindv = len(targetDict[i]["Participant Index"].columns)
        frequency = targetDict[i][xVariable].T.iat[-1,0]
        xFrequencies.append(frequency)

        data = targetDict[i]["Percentage Perceived Smoother"].loc["Percentage Perceived Smoother"]
        # filtered_data = reject_outliers(data, 2)
        filtered_data = data

        # print(len(data), len(filtered_data))

        total = 0
        for val in filtered_data:
            total += val
        
        mean = total/numJNDindv
        JNDdf.loc[i] = mean
    
    JNDdf.columns = ["Average Percentage Perceived Smoother"]
    JNDdf.index = xFrequencies
    JNDdf.loc[baseline] = [0]
    targetDict["Compiled Means"] = JNDdf

def compileJNDMulti():
    df = initEmptyDataFrame(len(JNDMulti0Inds), JNDTitles)
    df = pd.concat([JNDMulti0Dict["Compiled Means"], JNDMulti16Dict["Compiled Means"]], axis=1)
    df = pd.concat([df, JNDMulti33Dict["Compiled Means"]], axis=1)
    df = pd.concat([df, JNDMulti50Dict["Compiled Means"]], axis=1)
    df = pd.concat([df, JNDMulti100Dict["Compiled Means"]], axis=1)
    # df.loc[18] = 10
    # df.loc[17] = 10
    # df.loc[16] = 10
    # df.loc[15] = 10
    df.columns = JNDTitles
    return df

def compilePercentageDiff():
    for key in comparisonDict.keys():
        for ind in comparisonDict[key]["Haptic Frequency Difference Between Sides"]:
            val = comparisonDict[key]["Haptic Frequency Difference Between Sides"].iat[0, ind]
            percentageDiff = val/comparisonPatternsInt[key]
            comparisonDict[key]["Haptic Frequency Difference Between Sides"].iat[0, ind] = percentageDiff


#--------- READ FILES
comparisonDict = readCSV(comparisonDir, comparisonFile, comparisonInds, comparisonCols) 
multimodalDict = readCSV(multimodalDir, multimodalFile, multimodalInds, multimodalCols) 
JNDVisualDict = readCSV(JNDDir, JNDFile, JNDVisualInds, JNDCols) 
JNDHapticDict = readCSV(JNDDir, JNDFile, JNDHapticInds, JNDCols) 

JNDMulti0Dict = readCSV(JNDDir, JNDFile, JNDMulti0Inds, JNDCols) 
JNDMulti16Dict = readCSV(JNDDir, JNDFile, JNDMulti16Inds, JNDCols) 
JNDMulti33Dict = readCSV(JNDDir, JNDFile, JNDMulti33Inds, JNDCols) 
JNDMulti50Dict = readCSV(JNDDir, JNDFile, JNDMulti50Inds, JNDCols) 
JNDMulti100Dict = readCSV(JNDDir, JNDFile, JNDMulti100Inds, JNDCols) 

numComparisonPoints = comparisonDict[0]["Participant Index"].T.iat[-1,0] +1
numMultimodalPoints = multimodalDict["00"]["Participant Index"].T.iat[-1,0] +1
compileAccuracies()
compilePercentageDiff()

compileJND(JNDVisualInds, JNDVisualDict, "Trial Visual Frequency", JNDVisualBaseline)
compileJND(JNDHapticInds, JNDHapticDict, "Trial Haptic Frequency", JNDHapticBaseline)

compileJND(JNDMulti0Inds, JNDMulti0Dict, "Trial Haptic Frequency", JNDMultiHapticBaseline)
compileJND(JNDMulti16Inds, JNDMulti16Dict, "Trial Haptic Frequency", JNDMultiHapticBaseline)
compileJND(JNDMulti33Inds, JNDMulti33Dict, "Trial Haptic Frequency", JNDMultiHapticBaseline)
compileJND(JNDMulti50Inds, JNDMulti50Dict, "Trial Haptic Frequency", JNDMultiHapticBaseline)
compileJND(JNDMulti100Inds, JNDMulti100Dict, "Trial Haptic Frequency", JNDMultiHapticBaseline)

JNDMultidf = compileJNDMulti()

#--------- PLOT PARAMETERS
def definePlots():
    pltParameters = {
        "hapticFrequencyComparison_box": {
            "data": comparisonDict,
            "targetdf": 0,
            "indArray": comparisonInds_reordered,
            "numDataPoints": numComparisonPoints,
            "targetVariable": "Haptic Frequency Difference Between Sides",
            "suptitle": "Perceived Haptic Frequency Difference Between Sides",
            "subplt_titles": [""],
            "plt_num": 1,
            "xlabel": "Pattern",
            "ylabel": "Difference in Perceived Frequency (% Actual Frequency)",
            "xmin": 0,
            "xmax": 7,
            "ymin": 0,
            "ymax": 12,
            "xNames": comparisonPatterns
        },
        "hapticFrequencyComparison_dist": {
            "data": comparisonDict,
            "targetdf": 0,
            "indArray": comparisonInds_reordered,
            "numDataPoints": numComparisonPoints,
            "targetVariable": "Haptic Frequency Difference Between Sides",
            "suptitle": "Distribution",
            "subplt_titles": comparisonPatterns,
            "plt_num": 6,
            "xlabel": "Difference in Perceived Frequency (% Actual Frequency)",
            "ylabel": "Distribution",
            "xmin": 0,
            "xmax": 4,
            "ymin": 0,
            "ymax": 0.3,
            "xNames": ["20Hz", "110Hz", "200Hz"]
        },
        "multimodalAccuraciesRough_box": {
            "data": multimodalDict,
            "targetdf": 0,
            "indArray": multimodalInds_rough,
            "numDataPoints": numMultimodalPoints,
            "targetVariable": "Accuracies",
            "suptitle": "Rough Pattern",
            "subplt_titles": [""],
            "plt_num": 1,
            "xlabel": "Mode",
            "ylabel": "Classification Accuracy (%)",
            "xmin": 0,
            "xmax": 5,
            "ymin": 0,
            "ymax": 110,
            "xNames": multimodalTitles
        },
        "multimodalAccuraciesSmooth_box": {
            "data": multimodalDict,
            "targetdf": 0,
            "indArray": multimodalInds_smooth,
            "numDataPoints": numMultimodalPoints,
            "targetVariable": "Accuracies",
            "suptitle": "Smooth Pattern",
            "subplt_titles": [""],
            "plt_num": 1,
            "xlabel": "Mode",
            "ylabel": "Classification Accuracy (%)",
            "xmin": 0,
            "xmax": 5,
            "ymin": 0,
            "ymax": 110,
            "xNames": multimodalTitles
        },
        "multimodalTimeRough_box": {
            "data": multimodalDict,
            "targetdf": 0,
            "indArray": multimodalInds_rough,
            "numDataPoints": numMultimodalPoints*3,
            "targetVariable": "Response Time",
            "suptitle": "Rough Pattern",
            "subplt_titles": [""],
            "plt_num": 1,
            "xlabel": "Mode",
            "ylabel": "Response Time (s)",
            "xmin": 0,
            "xmax": 5,
            "ymin": 0,
            "ymax": 60,
            "xNames": multimodalTitles
        },
        "multimodalTimeSmooth_box": {
            "data": multimodalDict,
            "targetdf": 0,
            "indArray": multimodalInds_smooth,
            "numDataPoints": numMultimodalPoints*3,
            "targetVariable": "Response Time",
            "suptitle": "Smooth Pattern",
            "subplt_titles": [""],
            "plt_num": 1,
            "xlabel": "Mode",
            "ylabel": "Response Time (s)",
            "xmin": 0,
            "xmax": 5,
            "ymin": 0,
            "ymax": 60,
            "xNames": multimodalTitles
        },
        "JNDVisual_scatterline": {
            "data": JNDVisualDict,
            "targetdf": JNDVisualDict["Compiled Means"],
            "indArray": JNDVisualInds,
            "numDataPoints": 3, # polyfit parameter
            "targetVariable": "Average Percentage Perceived Smoother",
            "suptitle": "Unimodal Visual",
            "subplt_titles": [""],
            "plt_num": 1,
            "xlabel": "Frequency (Hz)",
            "ylabel": "Average Percentage Perceived Smoother (%)",
            "xmin":35,
            "xmax": 40,
            "ymin": 0,
            "ymax": 1.10,
            "xNames": np.arange(35, 41,1)
        },
        "JNDHaptic_scatterline": {
            "data": JNDHapticDict,
            "targetdf": JNDHapticDict["Compiled Means"].iloc[9:21],
            "indArray": JNDHapticInds,
            "numDataPoints": 3, # polyfit parameter
            "targetVariable": "Average Percentage Perceived Smoother",
            "suptitle": "Unimodal Haptic",
            "subplt_titles": [""],
            "plt_num": 1,
            "xlabel": "Frequency (Hz)",
            "ylabel": "Average Percentage Perceived Smoother (%)",
            "xmin":20,
            "xmax": 40,
            "ymin": 0,
            "ymax": 1.10,
            "xNames": np.arange(20, 41,5)
        },
        "JNDMulti_scatterline": {
            "data": JNDMultidf,
            "targetdf": JNDMultidf,
            "indArray": JNDMulti0Inds,
            "numDataPoints": 3, # polyfit parameter
            "targetVariable": JNDTitles,
            "suptitle": "Multimodal",
            "subplt_titles": [""],
            "plt_num": 1,
            "xlabel": "Frequency (Hz)",
            "ylabel": "Average Percentage Perceived Smoother (%)",
            "xmin":20,
            "xmax": 40,
            "ymin": 0,
            "ymax": 1.1,
            "xNames": np.arange(19, 41,5)
        }
    }
    return pltParameters

# ------------ PLOTTING FUNCTIONS
def createTargetArray(cur):
    # tempArray = []
    numDataPoints = cur["numDataPoints"]
    tempDataframe = initEmptyDataFrame(numDataPoints, cur["indArray"])
    tempDataframe.columns = cur["indArray"]
    # print(tempDataframe)

    for i in cur["indArray"]:
        tempData = cur["data"][i][cur["targetVariable"]].loc[cur["targetVariable"]].T
        # print(tempData)
        # tempArray = [tempArray, tempData]
        tempDataframe[i] = tempData
        # print(tempDataframe)
    
    return tempDataframe

def createSubsetDataframe(cur):
    # tempArray = []
    numDataPoints = cur["numDataPoints"]
    tempDataframe = initEmptyDataFrame(numDataPoints, cur["indArray"])

    for i in cur["indArray"]:
        tempData = cur["data"][cur["targetVariable"]][i]
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
        axsD[0,2].set_xlabel(cur["xlabel"])
        axsD[0,0].set_ylabel(cur["ylabel"])

def findWilcoxonSignRank(cur):
    for subplot in range((int)(cur["plt_num"]/2)):
        w, pvalue = stats.wilcoxon(cur["targetdf"][subplot], cur["targetdf"][subplot+3])
        print(cur["xNames"][subplot], w, pvalue)
    
def plotScatter(cur):
    figD, axsS = plt.subplots(ncols=cur["plt_num"], squeeze=False, figsize=(8,8))
    figD.suptitle(cur["suptitle"])

    for subplot in range(cur["plt_num"]):
        x = cur["targetdf"].index
        y = cur["targetdf"][cur["targetVariable"]]/100
        xEven = np.linspace(cur["xmin"], cur["xmax"], 50)

        axsS[0,subplot].scatter(x,y, ec='k') 

        yFit, JNDPoint, PSEPoint, coefficients = selectFit(cur["numDataPoints"], x, y, cur["xmin"], xEven)        
        axsS[0,subplot].plot(xEven,yFit) 

        print("JND: ", (100*(JNDPoint-cur["xmin"])/cur["xmin"]), "PSE: ", (100*(PSEPoint-cur["xmin"])/cur["xmin"]))
        printCoefficients(coefficients)

        xLine = np.append(x.values, 40)
        yLine = np.append(y.values, 110)

        axsS[0,subplot].plot(xLine, np.full((len(xLine),),0.75), "g:")
        axsS[0,subplot].plot(np.full((len(yLine),),JNDPoint), yLine, "g:")
        axsS[0,subplot].plot(xLine, np.full((len(xLine),),0.50), "r:")
        axsS[0,subplot].plot(np.full((len(yLine),),PSEPoint), yLine, "r:")

        axsS[0,subplot].set_xlim(cur["xmin"], cur["xmax"])
        axsS[0,subplot].set_ylim(cur["ymin"], cur["ymax"])

        axsS[0,subplot].set_xlabel(cur["xlabel"])
        axsS[0,0].set_ylabel(cur["ylabel"])
        # axsS[0,subplot].set_xticklabels(labels=cur["xNames"], fontsize=8, ha="right")
        axsS[0,subplot].set_xticks(cur["xNames"])
        axsS[0,subplot].set_yticklabels(["0","20","40","60","80","100"])
        
def plotMultiScatter(cur):
    figD, axsS = plt.subplots(ncols=cur["plt_num"], squeeze=False, figsize=(8,8))
    figD.suptitle(cur["suptitle"])

    for subplot in range(cur["plt_num"]):
        x = cur["targetdf"].index
        for target in cur["targetVariable"]:
            y = cur["targetdf"][target]/100
            xEven = np.linspace(cur["xmin"]-1, cur["xmax"], 50)
            yEven = np.linspace(cur["ymin"]-1, cur["ymax"], 21)

            # axsS[0,subplot].scatter(x,y, ec='k') # scatter dots

            yFit, JNDPoint, PSEPoint, coefficients = selectFit(cur["numDataPoints"], x, y, cur["xmin"], xEven)        
            axsS[0,subplot].plot(xEven,yFit, label=target) 

            print("JND: ", (100*(JNDPoint-cur["xmin"])/cur["xmin"]), "PSE: ", (100*(PSEPoint-cur["xmin"])/cur["xmin"]), " at ", PSEPoint)
            printCoefficients(coefficients)

        axsS[0,subplot].plot(np.full((len(yEven),),JNDMultiVisualEqBaseline), yEven, "k--", label="$Baseline Visual Frequency, f_v$")
        axsS[0,subplot].plot(np.full((len(yEven),),JNDMultiHapticBaseline), yEven, "r--", label="$Baseline Haptic Frequency, f_h$")

        axsS[0,subplot].set_xlim(cur["xmin"], cur["xmax"])
        axsS[0,subplot].set_ylim(cur["ymin"], cur["ymax"])

        axsS[0,subplot].set_xlabel(cur["xlabel"])
        axsS[0,0].set_ylabel(cur["ylabel"])
        # axsS[0,subplot].set_xticklabels(labels=cur["xNames"], fontsize=8, ha="right")
        axsS[0,subplot].set_xticks(cur["xNames"])
        # axsS[0,subplot].set_yticks(np.arange(0, 110, 10))
        axsS[0,subplot].set_yticklabels(["0","20","40","60","80","100"])

        axsS[0,subplot].legend()

def pf(x, alpha, beta):
    return 1. / (1 + np.exp( -(x-alpha)/beta))

def reversepf(y, alpha, beta):
    return alpha - beta*np.log((1/y)-1)

def selectFit(mode, x, y, baseline, xEven):
    if(mode == 0):
        coefficients = np.polyfit(x, y, 3)
        poly = np.poly1d(coefficients)
        yFit = poly(xEven)
        JNDPoint = (poly - 75).r
        PSEPoint = (poly - 50).r
    elif(mode == 1):
        coefficients = np.polyfit(np.log(x), y, 1)
        yFit = coefficients[0]*np.log(xEven) + coefficients[1]
        JNDPoint = np.exp((75 - coefficients[1])/coefficients[0])
        PSEPoint = np.exp((50 - coefficients[1])/coefficients[0])
    elif(mode == 2):
        coefficients = np.polyfit(x, y, 4)
        poly = np.poly1d(coefficients)
        yFit = poly(xEven)
        JNDPoint = (poly - 75).r
        PSEPoint = (poly - 50).r
    elif(mode == 3):
        cof0 = np.array([30., 1.])
        coefficients, ext = cf(pf, x, y, cof0)
        yFit = pf(xEven, coefficients[0], coefficients[1])
        JNDPoint = reversepf(0.75, coefficients[0], coefficients[1])
        PSEPoint = reversepf(0.50, coefficients[0], coefficients[1])
    
    if(isinstance(JNDPoint,np.ndarray)):
        JNDPoint = [np.real(d) for d in JNDPoint if np.isreal(d)]
        JNDPoint = [d for d in JNDPoint if d > baseline]
        JNDPoint = min(JNDPoint)
    if(isinstance(PSEPoint,np.ndarray)):
        PSEPoint = [np.real(p) for p in PSEPoint if np.isreal(p)]    
        PSEPoint = [p for p in PSEPoint if p > baseline]    
        PSEPoint = min(PSEPoint)
    
    
    return yFit, JNDPoint, PSEPoint, coefficients

def printCoefficients(coefs):
    for c in coefs:
        print(c)


#----------- CALL PLOT FUNCTIONS
plt.close('all')
plotParameters = definePlots()

plotParameters["hapticFrequencyComparison_box"]["targetdf"] = createTargetArray(plotParameters["hapticFrequencyComparison_box"])
plotParameters["hapticFrequencyComparison_dist"]["targetdf"] = createTargetArray(plotParameters["hapticFrequencyComparison_dist"])

# plotParameters["multimodalAccuraciesRough_box"]["targetdf"] = createSubsetDataframe(plotParameters["multimodalAccuraciesRough_box"])
# plotParameters["multimodalAccuraciesSmooth_box"]["targetdf"] = createSubsetDataframe(plotParameters["multimodalAccuraciesSmooth_box"])

# plotParameters["multimodalTimeRough_box"]["targetdf"] = createTargetArray(plotParameters["multimodalTimeRough_box"])
# plotParameters["multimodalTimeSmooth_box"]["targetdf"] = createTargetArray(plotParameters["multimodalTimeSmooth_box"])

# PLOT INDV RESULT
# plotParameters["hapticFrequencyComparison_box"]["targetdf"] = pd.DataFrame(plotParameters["hapticFrequencyComparison_box"]["targetdf"].iloc[14]).T

# print(type(targetdf))

#--- PLOT BOX
# plotBox(plotParameters["hapticFrequencyComparison_box"])

# plotBox(plotParameters["multimodalAccuraciesRough_box"])
# plotBox(plotParameters["multimodalAccuraciesSmooth_box"])

# plotBox(plotParameters["multimodalTimeRough_box"])
# plotBox(plotParameters["multimodalTimeSmooth_box"])

# could create an analysis for percentage of people who thought X was rougher than Y
# ie. do a check between left and right results for each pair

#--- CHECK DISTRIBUTION (PLOT HISTOGRAMS)
# checkDistribution(plotParameters["hapticFrequencyComparison_dist"])

#--- RUN WILCOXON SIGN RANK TEST
# findWilcoxonSignRank(plotParameters["hapticFrequencyComparison_dist"])

#--- PLOT SCATTER AND LINE FOR JND
# plotScatter(plotParameters["JNDVisual_scatterline"])
# plotScatter(plotParameters["JNDHaptic_scatterline"])

plotMultiScatter(plotParameters["JNDMulti_scatterline"])

#------------- SHOW PLOTS
# to calculate weights: point - 20 / 12 = visual weight
# plt.rc({'axes.titlesize': 50, 'lines.linewidth': 3})
# plt.rc('axes', titlesize=50)
plt.show()

print("done")
