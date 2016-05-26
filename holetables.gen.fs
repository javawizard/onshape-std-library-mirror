FeatureScript 355; /* Automatically generated version */
/* Automatically generated file -- DO NOT EDIT */

import(path : "onshape/std/units.fs", version : "355.0");
import(path : "onshape/std/lookupTablePath.fs", version : "355.0");

const ANSI_drillTable = {
    "name" : "size",
    "displayName" : "Drill size",
    "default" : "1/4",
    "entries" : {
        "#80" : {"holeDiameter" : "0.0135 * inch", "tapDrillDiameter" : "0.0135 * inch"},
        "#79" : {"holeDiameter" : "0.0145 * inch", "tapDrillDiameter" : "0.0145 * inch"},
        "1/64" : {"holeDiameter" : "1/64 * inch", "tapDrillDiameter" : "1/64 * inch"},
        "#78" : {"holeDiameter" : "0.016 * inch", "tapDrillDiameter" : "0.016 * inch"},
        "#77" : {"holeDiameter" : "0.018 * inch", "tapDrillDiameter" : "0.018 * inch"},
        "#76" : {"holeDiameter" : "0.02 * inch", "tapDrillDiameter" : "0.02 * inch"},
        "#75" : {"holeDiameter" : "0.021 * inch", "tapDrillDiameter" : "0.021 * inch"},
        "#74" : {"holeDiameter" : "0.0225 * inch", "tapDrillDiameter" : "0.0225 * inch"},
        "#73" : {"holeDiameter" : "0.024 * inch", "tapDrillDiameter" : "0.024 * inch"},
        "#72" : {"holeDiameter" : "0.025 * inch", "tapDrillDiameter" : "0.025 * inch"},
        "#71" : {"holeDiameter" : "0.026 * inch", "tapDrillDiameter" : "0.026 * inch"},
        "#70" : {"holeDiameter" : "0.028 * inch", "tapDrillDiameter" : "0.028 * inch"},
        "#69" : {"holeDiameter" : "0.0292 * inch", "tapDrillDiameter" : "0.0292 * inch"},
        "#68" : {"holeDiameter" : "0.031 * inch", "tapDrillDiameter" : "0.031 * inch"},
        "1/32" : {"holeDiameter" : "1/32 * inch", "tapDrillDiameter" : "1/32 * inch"},
        "#67" : {"holeDiameter" : "0.032 * inch", "tapDrillDiameter" : "0.032 * inch"},
        "#66" : {"holeDiameter" : "0.033 * inch", "tapDrillDiameter" : "0.033 * inch"},
        "#65" : {"holeDiameter" : "0.035 * inch", "tapDrillDiameter" : "0.035 * inch"},
        "#64" : {"holeDiameter" : "0.036 * inch", "tapDrillDiameter" : "0.036 * inch"},
        "#63" : {"holeDiameter" : "0.037 * inch", "tapDrillDiameter" : "0.037 * inch"},
        "#62" : {"holeDiameter" : "0.038 * inch", "tapDrillDiameter" : "0.038 * inch"},
        "#61" : {"holeDiameter" : "0.039 * inch", "tapDrillDiameter" : "0.039 * inch"},
        "#60" : {"holeDiameter" : "0.04 * inch", "tapDrillDiameter" : "0.04 * inch"},
        "#59" : {"holeDiameter" : "0.041 * inch", "tapDrillDiameter" : "0.041 * inch"},
        "#58" : {"holeDiameter" : "0.042 * inch", "tapDrillDiameter" : "0.042 * inch"},
        "#57" : {"holeDiameter" : "0.043 * inch", "tapDrillDiameter" : "0.043 * inch"},
        "#56" : {"holeDiameter" : "0.0465 * inch", "tapDrillDiameter" : "0.0465 * inch"},
        "3/64" : {"holeDiameter" : "3/64 * inch", "tapDrillDiameter" : "3/64 * inch"},
        "#55" : {"holeDiameter" : "0.052 * inch", "tapDrillDiameter" : "0.052 * inch"},
        "#54" : {"holeDiameter" : "0.055 * inch", "tapDrillDiameter" : "0.055 * inch"},
        "#53" : {"holeDiameter" : "0.0595 * inch", "tapDrillDiameter" : "0.0595 * inch"},
        "1/16" : {"holeDiameter" : "1/16 * inch", "tapDrillDiameter" : "1/16 * inch"},
        "#52" : {"holeDiameter" : "0.0635 * inch", "tapDrillDiameter" : "0.0635 * inch"},
        "#51" : {"holeDiameter" : "0.067 * inch", "tapDrillDiameter" : "0.067 * inch"},
        "#50" : {"holeDiameter" : "0.07 * inch", "tapDrillDiameter" : "0.07 * inch"},
        "#49" : {"holeDiameter" : "0.073 * inch", "tapDrillDiameter" : "0.073 * inch"},
        "#48" : {"holeDiameter" : "0.076 * inch", "tapDrillDiameter" : "0.076 * inch"},
        "5/64" : {"holeDiameter" : "5/64 * inch", "tapDrillDiameter" : "5/64 * inch"},
        "#47" : {"holeDiameter" : "0.0785 * inch", "tapDrillDiameter" : "0.0785 * inch"},
        "#46" : {"holeDiameter" : "0.081 * inch", "tapDrillDiameter" : "0.081 * inch"},
        "#45" : {"holeDiameter" : "0.082 * inch", "tapDrillDiameter" : "0.082 * inch"},
        "#44" : {"holeDiameter" : "0.086 * inch", "tapDrillDiameter" : "0.086 * inch"},
        "#43" : {"holeDiameter" : "0.089 * inch", "tapDrillDiameter" : "0.089 * inch"},
        "#42" : {"holeDiameter" : "0.0935 * inch", "tapDrillDiameter" : "0.0935 * inch"},
        "3/32" : {"holeDiameter" : "3/32 * inch", "tapDrillDiameter" : "3/32 * inch"},
        "#41" : {"holeDiameter" : "0.096 * inch", "tapDrillDiameter" : "0.096 * inch"},
        "#40" : {"holeDiameter" : "0.098 * inch", "tapDrillDiameter" : "0.098 * inch"},
        "#39" : {"holeDiameter" : "0.0995 * inch", "tapDrillDiameter" : "0.0995 * inch"},
        "#38" : {"holeDiameter" : "0.1015 * inch", "tapDrillDiameter" : "0.1015 * inch"},
        "#37" : {"holeDiameter" : "0.104 * inch", "tapDrillDiameter" : "0.104 * inch"},
        "#36" : {"holeDiameter" : "0.1065 * inch", "tapDrillDiameter" : "0.1065 * inch"},
        "7/64" : {"holeDiameter" : "7/64 * inch", "tapDrillDiameter" : "7/64 * inch"},
        "#35" : {"holeDiameter" : "0.11 * inch", "tapDrillDiameter" : "0.11 * inch"},
        "#34" : {"holeDiameter" : "0.111 * inch", "tapDrillDiameter" : "0.111 * inch"},
        "#33" : {"holeDiameter" : "0.113 * inch", "tapDrillDiameter" : "0.113 * inch"},
        "#32" : {"holeDiameter" : "0.116 * inch", "tapDrillDiameter" : "0.116 * inch"},
        "#31" : {"holeDiameter" : "0.12 * inch", "tapDrillDiameter" : "0.12 * inch"},
        "1/8" : {"holeDiameter" : "1/8 * inch", "tapDrillDiameter" : "1/8 * inch"},
        "#30" : {"holeDiameter" : "0.1285 * inch", "tapDrillDiameter" : "0.1285 * inch"},
        "#29" : {"holeDiameter" : "0.136 * inch", "tapDrillDiameter" : "0.136 * inch"},
        "#28" : {"holeDiameter" : "0.1405 * inch", "tapDrillDiameter" : "0.1405 * inch"},
        "9/64" : {"holeDiameter" : "9/64 * inch", "tapDrillDiameter" : "9/64 * inch"},
        "#27" : {"holeDiameter" : "0.144 * inch", "tapDrillDiameter" : "0.144 * inch"},
        "#26" : {"holeDiameter" : "0.147 * inch", "tapDrillDiameter" : "0.147 * inch"},
        "#25" : {"holeDiameter" : "0.1495 * inch", "tapDrillDiameter" : "0.1495 * inch"},
        "#24" : {"holeDiameter" : "0.152 * inch", "tapDrillDiameter" : "0.152 * inch"},
        "#23" : {"holeDiameter" : "0.154 * inch", "tapDrillDiameter" : "0.154 * inch"},
        "5/32" : {"holeDiameter" : "5/32 * inch", "tapDrillDiameter" : "5/32 * inch"},
        "#22" : {"holeDiameter" : "0.157 * inch", "tapDrillDiameter" : "0.157 * inch"},
        "#21" : {"holeDiameter" : "0.159 * inch", "tapDrillDiameter" : "0.159 * inch"},
        "#20" : {"holeDiameter" : "0.161 * inch", "tapDrillDiameter" : "0.161 * inch"},
        "#19" : {"holeDiameter" : "0.166 * inch", "tapDrillDiameter" : "0.166 * inch"},
        "#18" : {"holeDiameter" : "0.1695 * inch", "tapDrillDiameter" : "0.1695 * inch"},
        "11/64" : {"holeDiameter" : "11/64 * inch", "tapDrillDiameter" : "11/64 * inch"},
        "#17" : {"holeDiameter" : "0.173 * inch", "tapDrillDiameter" : "0.173 * inch"},
        "#16" : {"holeDiameter" : "0.177 * inch", "tapDrillDiameter" : "0.177 * inch"},
        "#15" : {"holeDiameter" : "0.18 * inch", "tapDrillDiameter" : "0.18 * inch"},
        "#14" : {"holeDiameter" : "0.182 * inch", "tapDrillDiameter" : "0.182 * inch"},
        "#13" : {"holeDiameter" : "0.185 * inch", "tapDrillDiameter" : "0.185 * inch"},
        "3/16" : {"holeDiameter" : "3/16 * inch", "tapDrillDiameter" : "3/16 * inch"},
        "#12" : {"holeDiameter" : "0.189 * inch", "tapDrillDiameter" : "0.189 * inch"},
        "#11" : {"holeDiameter" : "0.191 * inch", "tapDrillDiameter" : "0.191 * inch"},
        "#10" : {"holeDiameter" : "0.1935 * inch", "tapDrillDiameter" : "0.1935 * inch"},
        "#9" : {"holeDiameter" : "0.196 * inch", "tapDrillDiameter" : "0.196 * inch"},
        "#8" : {"holeDiameter" : "0.199 * inch", "tapDrillDiameter" : "0.199 * inch"},
        "#7" : {"holeDiameter" : "0.201 * inch", "tapDrillDiameter" : "0.201 * inch"},
        "13/64" : {"holeDiameter" : "13/64 * inch", "tapDrillDiameter" : "13/64 * inch"},
        "#6" : {"holeDiameter" : "0.204 * inch", "tapDrillDiameter" : "0.204 * inch"},
        "#5" : {"holeDiameter" : "0.2055 * inch", "tapDrillDiameter" : "0.2055 * inch"},
        "#4" : {"holeDiameter" : "0.209 * inch", "tapDrillDiameter" : "0.209 * inch"},
        "#3" : {"holeDiameter" : "0.213 * inch", "tapDrillDiameter" : "0.213 * inch"},
        "7/32" : {"holeDiameter" : "7/32 * inch", "tapDrillDiameter" : "7/32 * inch"},
        "#2" : {"holeDiameter" : "0.221 * inch", "tapDrillDiameter" : "0.221 * inch"},
        "#1" : {"holeDiameter" : "0.228 * inch", "tapDrillDiameter" : "0.228 * inch"},
        "A" : {"holeDiameter" : "0.234 * inch", "tapDrillDiameter" : "0.234 * inch"},
        "15/64" : {"holeDiameter" : "15/64 * inch", "tapDrillDiameter" : "15/64 * inch"},
        "B" : {"holeDiameter" : "0.238 * inch", "tapDrillDiameter" : "0.238 * inch"},
        "C" : {"holeDiameter" : "0.242 * inch", "tapDrillDiameter" : "0.242 * inch"},
        "D" : {"holeDiameter" : "0.246 * inch", "tapDrillDiameter" : "0.246 * inch"},
        "1/4" : {"holeDiameter" : "1/4 * inch", "tapDrillDiameter" : "1/4 * inch"},
        "E" : {"holeDiameter" : "0.25 * inch", "tapDrillDiameter" : "0.25 * inch"},
        "F" : {"holeDiameter" : "0.257 * inch", "tapDrillDiameter" : "0.257 * inch"},
        "G" : {"holeDiameter" : "0.261 * inch", "tapDrillDiameter" : "0.261 * inch"},
        "17/64" : {"holeDiameter" : "17/64 * inch", "tapDrillDiameter" : "17/64 * inch"},
        "H" : {"holeDiameter" : "0.266 * inch", "tapDrillDiameter" : "0.266 * inch"},
        "I" : {"holeDiameter" : "0.272 * inch", "tapDrillDiameter" : "0.272 * inch"},
        "J" : {"holeDiameter" : "0.277 * inch", "tapDrillDiameter" : "0.277 * inch"},
        "K" : {"holeDiameter" : "0.2811 * inch", "tapDrillDiameter" : "0.2811 * inch"},
        "9/32" : {"holeDiameter" : "9/32 * inch", "tapDrillDiameter" : "9/32 * inch"},
        "L" : {"holeDiameter" : "0.29 * inch", "tapDrillDiameter" : "0.29 * inch"},
        "M" : {"holeDiameter" : "0.295 * inch", "tapDrillDiameter" : "0.295 * inch"},
        "19/64" : {"holeDiameter" : "19/64 * inch", "tapDrillDiameter" : "19/64 * inch"},
        "N" : {"holeDiameter" : "0.302 * inch", "tapDrillDiameter" : "0.302 * inch"},
        "5/16" : {"holeDiameter" : "5/16 * inch", "tapDrillDiameter" : "5/16 * inch"},
        "O" : {"holeDiameter" : "0.316 * inch", "tapDrillDiameter" : "0.316 * inch"},
        "P" : {"holeDiameter" : "0.323 * inch", "tapDrillDiameter" : "0.323 * inch"},
        "21/64" : {"holeDiameter" : "21/64 * inch", "tapDrillDiameter" : "21/64 * inch"},
        "Q" : {"holeDiameter" : "0.332 * inch", "tapDrillDiameter" : "0.332 * inch"},
        "R" : {"holeDiameter" : "0.339 * inch", "tapDrillDiameter" : "0.339 * inch"},
        "11/32" : {"holeDiameter" : "11/32 * inch", "tapDrillDiameter" : "11/32 * inch"},
        "S" : {"holeDiameter" : "0.348 * inch", "tapDrillDiameter" : "0.348 * inch"},
        "T" : {"holeDiameter" : "0.358 * inch", "tapDrillDiameter" : "0.358 * inch"},
        "23/64" : {"holeDiameter" : "23/64 * inch", "tapDrillDiameter" : "23/64 * inch"},
        "U" : {"holeDiameter" : "0.368 * inch", "tapDrillDiameter" : "0.368 * inch"},
        "3/8" : {"holeDiameter" : "3/8 * inch", "tapDrillDiameter" : "3/8 * inch"},
        "V" : {"holeDiameter" : "0.377 * inch", "tapDrillDiameter" : "0.377 * inch"},
        "W" : {"holeDiameter" : "0.386 * inch", "tapDrillDiameter" : "0.386 * inch"},
        "25/64" : {"holeDiameter" : "25/64 * inch", "tapDrillDiameter" : "25/64 * inch"},
        "X" : {"holeDiameter" : "0.397 * inch", "tapDrillDiameter" : "0.397 * inch"},
        "Y" : {"holeDiameter" : "0.404 * inch", "tapDrillDiameter" : "0.404 * inch"},
        "13/32" : {"holeDiameter" : "13/32 * inch", "tapDrillDiameter" : "13/32 * inch"},
        "Z" : {"holeDiameter" : "0.413 * inch", "tapDrillDiameter" : "0.413 * inch"},
        "27/64" : {"holeDiameter" : "27/64 * inch", "tapDrillDiameter" : "27/64 * inch"},
        "7/16" : {"holeDiameter" : "7/16 * inch", "tapDrillDiameter" : "7/16 * inch"},
        "29/64" : {"holeDiameter" : "29/64 * inch", "tapDrillDiameter" : "29/64 * inch"},
        "15/32" : {"holeDiameter" : "15/32 * inch", "tapDrillDiameter" : "15/32 * inch"},
        "31/64" : {"holeDiameter" : "31/64 * inch", "tapDrillDiameter" : "31/64 * inch"},
        "1/2" : {"holeDiameter" : "1/2 * inch", "tapDrillDiameter" : "1/2 * inch"},
        "33/64" : {"holeDiameter" : "33/64 * inch", "tapDrillDiameter" : "33/64 * inch"},
        "17/32" : {"holeDiameter" : "17/32 * inch", "tapDrillDiameter" : "17/32 * inch"},
        "35/64" : {"holeDiameter" : "35/64 * inch", "tapDrillDiameter" : "35/64 * inch"},
        "9/16" : {"holeDiameter" : "9/16 * inch", "tapDrillDiameter" : "9/16 * inch"},
        "37/64" : {"holeDiameter" : "37/64 * inch", "tapDrillDiameter" : "37/64 * inch"},
        "19/32" : {"holeDiameter" : "19/32 * inch", "tapDrillDiameter" : "19/32 * inch"},
        "39/64" : {"holeDiameter" : "39/64 * inch", "tapDrillDiameter" : "39/64 * inch"},
        "5/8" : {"holeDiameter" : "5/8 * inch", "tapDrillDiameter" : "5/8 * inch"},
        "41/64" : {"holeDiameter" : "41/64 * inch", "tapDrillDiameter" : "41/64 * inch"},
        "21/32" : {"holeDiameter" : "21/32 * inch", "tapDrillDiameter" : "21/32 * inch"},
        "43/64" : {"holeDiameter" : "43/64 * inch", "tapDrillDiameter" : "43/64 * inch"},
        "11/16" : {"holeDiameter" : "11/16 * inch", "tapDrillDiameter" : "11/16 * inch"},
        "45/64" : {"holeDiameter" : "45/64 * inch", "tapDrillDiameter" : "45/64 * inch"},
        "23/32" : {"holeDiameter" : "23/32 * inch", "tapDrillDiameter" : "23/32 * inch"},
        "47/64" : {"holeDiameter" : "47/64 * inch", "tapDrillDiameter" : "47/64 * inch"},
        "3/4" : {"holeDiameter" : "3/4 * inch", "tapDrillDiameter" : "3/4 * inch"},
        "49/64" : {"holeDiameter" : "49/64 * inch", "tapDrillDiameter" : "49/64 * inch"},
        "25/32" : {"holeDiameter" : "25/32 * inch", "tapDrillDiameter" : "25/32 * inch"},
        "51/64" : {"holeDiameter" : "51/64 * inch", "tapDrillDiameter" : "51/64 * inch"},
        "13/16" : {"holeDiameter" : "13/16 * inch", "tapDrillDiameter" : "13/16 * inch"},
        "53/64" : {"holeDiameter" : "53/64 * inch", "tapDrillDiameter" : "53/64 * inch"},
        "27/32" : {"holeDiameter" : "27/32 * inch", "tapDrillDiameter" : "27/32 * inch"},
        "55/64" : {"holeDiameter" : "55/64 * inch", "tapDrillDiameter" : "55/64 * inch"},
        "7/8" : {"holeDiameter" : "7/8 * inch", "tapDrillDiameter" : "7/8 * inch"},
        "57/64" : {"holeDiameter" : "57/64 * inch", "tapDrillDiameter" : "57/64 * inch"},
        "29/32" : {"holeDiameter" : "29/32 * inch", "tapDrillDiameter" : "29/32 * inch"},
        "59/64" : {"holeDiameter" : "59/64 * inch", "tapDrillDiameter" : "59/64 * inch"},
        "15/16" : {"holeDiameter" : "15/16 * inch", "tapDrillDiameter" : "15/16 * inch"},
        "61/64" : {"holeDiameter" : "61/64 * inch", "tapDrillDiameter" : "61/64 * inch"},
        "31/32" : {"holeDiameter" : "31/32 * inch", "tapDrillDiameter" : "31/32 * inch"},
        "63/64" : {"holeDiameter" : "63/64 * inch", "tapDrillDiameter" : "63/64 * inch"},
        "1" : {"holeDiameter" : "1 * inch", "tapDrillDiameter" : "1 * inch"}
    }
};

const ANSI_ClearanceHoleTable = {
    "name" : "size",
    "displayName" : "Size",
    "default" : "1/4",
    "entries" : {
        "#0" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.064 * inch", "cBoreDiameter" : "1/8 * inch", "cBoreDepth" : "0.06 * inch", "cSinkDiameter" : "0.138 * inch", "cSinkAngle" : "82 * degree"},
                "Free" : {"holeDiameter" : "0.07 * inch", "cBoreDiameter" : "1/8 * inch", "cBoreDepth" : "0.06 * inch", "cSinkDiameter" : "0.138 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "#1" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.076 * inch", "cBoreDiameter" : "5/32 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree"},
                "Free" : {"holeDiameter" : "0.081 * inch", "cBoreDiameter" : "5/32 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "#2" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.089 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree"},
                "Free" : {"holeDiameter" : "0.096 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "#3" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.104 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree"},
                "Free" : {"holeDiameter" : "0.11 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "#4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.116 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree"},
                "Free" : {"holeDiameter" : "0.1285 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "#5" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.129 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree"},
                "Free" : {"holeDiameter" : "0.136 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "#6" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.144 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree"},
                "Free" : {"holeDiameter" : "0.1495 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "#8" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.170 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree"},
                "Free" : {"holeDiameter" : "0.177 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "#10" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.196 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree"},
                "Free" : {"holeDiameter" : "0.201 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "1/4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.257 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree"},
                "Free" : {"holeDiameter" : "0.266 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "5/16" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.323 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree"},
                "Free" : {"holeDiameter" : "0.332 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "3/8" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.386 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree"},
                "Free" : {"holeDiameter" : "0.397 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "7/16" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.453 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree"},
                "Free" : {"holeDiameter" : "0.4687 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "1/2" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.516 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree"},
                "Free" : {"holeDiameter" : "0.5312 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "5/8" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.641 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree"},
                "Free" : {"holeDiameter" : "0.6562 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "3/4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.766 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree"},
                "Free" : {"holeDiameter" : "0.7812 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "7/8" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.891 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree"},
                "Free" : {"holeDiameter" : "0.9062 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "1" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "1.016 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree"},
                "Free" : {"holeDiameter" : "1.0313 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "1 1/4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "1.2656 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree"},
                "Free" : {"holeDiameter" : "1.2188 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "1 1/2" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "1.5156 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree"},
                "Free" : {"holeDiameter" : "1.5312 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree"}
            }
        }
    }
};

const ANSI_TappedHoleTable = {
    "name" : "size",
    "displayName" : "Size",
    "default" : "1/4",
    "entries" : {
        "#0" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "80 tpi",
            "entries" : {
                "80 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.052 * inch", "tapDrillDiameter" : "0.052 * inch", "cBoreDiameter" : "1/8 * inch", "cBoreDepth" : "0.06 * inch", "cSinkDiameter" : "0.138 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.0469 * inch", "tapDrillDiameter" : "0.0469 * inch", "cBoreDiameter" : "1/8 * inch", "cBoreDepth" : "0.06 * inch", "cSinkDiameter" : "0.138 * inch", "cSinkAngle" : "82 * degree"}
                    }
                }
            }
        },
        "#1" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "64 tpi",
            "entries" : {
                "64 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.0625 * inch", "tapDrillDiameter" : "0.0625 * inch", "cBoreDiameter" : "5/32 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.0595 * inch", "tapDrillDiameter" : "0.0595 * inch", "cBoreDiameter" : "5/32 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "72 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.0635 * inch", "tapDrillDiameter" : "0.0635 * inch", "cBoreDiameter" : "5/33 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.0595 * inch", "tapDrillDiameter" : "0.0595 * inch", "cBoreDiameter" : "5/33 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree"}
                    }
                }
            }
        },
        "#2" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "56 tpi",
            "entries" : {
                "56 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.073 * inch", "tapDrillDiameter" : "0.073 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.07 * inch", "tapDrillDiameter" : "0.07 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "64 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.076 * inch", "tapDrillDiameter" : "0.076 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.07 * inch", "tapDrillDiameter" : "0.07 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree"}
                    }
                }
            }
        },
        "#3" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "48 tpi",
            "entries" : {
                "48 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.086 * inch", "tapDrillDiameter" : "0.086 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.0785 * inch", "tapDrillDiameter" : "0.0785 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "56 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.089 * inch", "tapDrillDiameter" : "0.089 * inch", "cBoreDiameter" : "7/33 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.082 * inch", "tapDrillDiameter" : "0.082 * inch", "cBoreDiameter" : "7/33 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree"}
                    }
                }
            }
        },
        "#4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "40 tpi",
            "entries" : {
                "40 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.096 * inch", "tapDrillDiameter" : "0.096 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.089 * inch", "tapDrillDiameter" : "0.089 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "48 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.098 * inch", "tapDrillDiameter" : "0.098 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.0935 * inch", "tapDrillDiameter" : "0.0935 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree"}
                    }
                }
            }
        },
        "#5" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "40 tpi",
            "entries" : {
                "40 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.1094 * inch", "tapDrillDiameter" : "0.1094 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.1015 * inch", "tapDrillDiameter" : "0.1015 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "44 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.11 * inch", "tapDrillDiameter" : "0.11 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.104 * inch", "tapDrillDiameter" : "0.104 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree"}
                    }
                }
            }
        },
        "#6" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "32 tpi",
            "entries" : {
                "32 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.116 * inch", "tapDrillDiameter" : "0.116 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.1065 * inch", "tapDrillDiameter" : "0.1065 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "40 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.12 * inch", "tapDrillDiameter" : "0.12 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.113 * inch", "tapDrillDiameter" : "0.113 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree"}
                    }
                }
            }
        },
        "#8" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "32 tpi",
            "entries" : {
                "32 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.144 * inch", "tapDrillDiameter" : "0.144 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.136 * inch", "tapDrillDiameter" : "0.136 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "36 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.147 * inch", "tapDrillDiameter" : "0.147 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.136 * inch", "tapDrillDiameter" : "0.136 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree"}
                    }
                }
            }
        },
        "#10" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "24 tpi",
            "entries" : {
                "24 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.161 * inch", "tapDrillDiameter" : "0.161 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.1495 * inch", "tapDrillDiameter" : "0.1495 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "32 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.1695 * inch", "tapDrillDiameter" : "0.1695 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.159 * inch", "tapDrillDiameter" : "0.159 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree"}
                    }
                }
            }
        },
        "1/4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "28 tpi",
            "entries" : {
                "20 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.2188 * inch", "tapDrillDiameter" : "0.2188 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.201 * inch", "tapDrillDiameter" : "0.201 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "28 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.228 * inch", "tapDrillDiameter" : "0.228 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.213 * inch", "tapDrillDiameter" : "0.213 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "32 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.228 * inch", "tapDrillDiameter" : "0.228 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.2188 * inch", "tapDrillDiameter" : "0.2188 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree"}
                    }
                }
            }
        },
        "5/16" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "24 tpi",
            "entries" : {
                "18 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.277 * inch", "tapDrillDiameter" : "0.277 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.257 * inch", "tapDrillDiameter" : "0.257 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "24 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.2812 * inch", "tapDrillDiameter" : "0.2812 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.272 * inch", "tapDrillDiameter" : "0.272 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "32 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.29 * inch", "tapDrillDiameter" : "0.29 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.2812 * inch", "tapDrillDiameter" : "0.2812 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree"}
                    }
                }
            }
        },
        "3/8" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "24 tpi",
            "entries" : {
                "16 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.332 * inch", "tapDrillDiameter" : "0.332 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.3125 * inch", "tapDrillDiameter" : "0.3125 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "24 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.348 * inch", "tapDrillDiameter" : "0.348 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.332 * inch", "tapDrillDiameter" : "0.332 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "32 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.358 * inch", "tapDrillDiameter" : "0.358 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.3438 * inch", "tapDrillDiameter" : "0.3438 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree"}
                    }
                }
            }
        },
        "7/16" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "20 tpi",
            "entries" : {
                "14 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.3906 * inch", "tapDrillDiameter" : "0.3906 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.368 * inch", "tapDrillDiameter" : "0.368 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "20 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.4062 * inch", "tapDrillDiameter" : "0.4062 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.3906 * inch", "tapDrillDiameter" : "0.3906 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "28 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.413 * inch", "tapDrillDiameter" : "0.413 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.404 * inch", "tapDrillDiameter" : "0.404 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree"}
                    }
                }
            }
        },
        "1/2" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "20 tpi",
            "entries" : {
                "13 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.4531 * inch", "tapDrillDiameter" : "0.4531 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.4219 * inch", "tapDrillDiameter" : "0.4219 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "20 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.4688 * inch", "tapDrillDiameter" : "0.4688 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.4531 * inch", "tapDrillDiameter" : "0.4531 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "28 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.4688 * inch", "tapDrillDiameter" : "0.4688 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.4688 * inch", "tapDrillDiameter" : "0.4688 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree"}
                    }
                }
            }
        },
        "5/8" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "18 tpi",
            "entries" : {
                "11 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.5625 * inch", "tapDrillDiameter" : "0.5625 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.5312 * inch", "tapDrillDiameter" : "0.5312 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "18 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.5938 * inch", "tapDrillDiameter" : "0.5938 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.5781 * inch", "tapDrillDiameter" : "0.5781 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "24 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.5938 * inch", "tapDrillDiameter" : "0.5938 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.5781 * inch", "tapDrillDiameter" : "0.5781 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree"}
                    }
                }
            }
        },
        "3/4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "16 tpi",
            "entries" : {
                "10 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.6875 * inch", "tapDrillDiameter" : "0.6875 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.6562 * inch", "tapDrillDiameter" : "0.6562 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "16 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.7031 * inch", "tapDrillDiameter" : "0.7031 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.6875 * inch", "tapDrillDiameter" : "0.6875 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "20 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.7188 * inch", "tapDrillDiameter" : "0.7188 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.7031 * inch", "tapDrillDiameter" : "0.7031 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree"}
                    }
                }
            }
        },
        "7/8" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "14 tpi",
            "entries" : {
                "9 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.7969 * inch", "tapDrillDiameter" : "0.7969 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.7656 * inch", "tapDrillDiameter" : "0.7656 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "14 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.8281 * inch", "tapDrillDiameter" : "0.8281 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.8125 * inch", "tapDrillDiameter" : "0.8125 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "20 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.8438 * inch", "tapDrillDiameter" : "0.8438 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.8281 * inch", "tapDrillDiameter" : "0.8281 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree"}
                    }
                }
            }
        },
        "1" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "12 tpi",
            "entries" : {
                "8 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.9219 * inch", "tapDrillDiameter" : "0.9219 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.875 * inch", "tapDrillDiameter" : "0.875 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.9531 * inch", "tapDrillDiameter" : "0.9531 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.9219 * inch", "tapDrillDiameter" : "0.9219 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "20 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.9688 * inch", "tapDrillDiameter" : "0.9688 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "0.9531 * inch", "tapDrillDiameter" : "0.9531 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree"}
                    }
                }
            }
        },
        "1 1/4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "12 tpi",
            "entries" : {
                "7 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.1562 * inch", "tapDrillDiameter" : "1.1562 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "1.1094 * inch", "tapDrillDiameter" : "1.1094 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.2031 * inch", "tapDrillDiameter" : "1.2031 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "1.1719 * inch", "tapDrillDiameter" : "1.1719 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "18 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.2187 * inch", "tapDrillDiameter" : "1.2187 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "1.1875 * inch", "tapDrillDiameter" : "1.1875 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree"}
                    }
                }
            }
        },
        "1 1/2" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "12 tpi",
            "entries" : {
                "6 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.3906 * inch", "tapDrillDiameter" : "1.3906 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "1.3437 * inch", "tapDrillDiameter" : "1.3437 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.4375 * inch", "tapDrillDiameter" : "1.4375 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "1.4219 * inch", "tapDrillDiameter" : "1.4219 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree"}
                    }
                },
                "18 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.4687 * inch", "tapDrillDiameter" : "1.4687 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree"},
                        "75%" : {"holeDiameter" : "1.4375 * inch", "tapDrillDiameter" : "1.4375 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree"}
                    }
                }
            }
        }
    }
};

const ANSI_ThroughTappedScrewTable = {
    "name" : "size",
    "displayName" : "Size",
    "default" : "1/4",
    "entries" : {
        "#0" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "80 tpi",
            "entries" : {
                "80 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.052 * inch", "holeDiameter" : "0.064 * inch", "cBoreDiameter" : "1/8 * inch", "cBoreDepth" : "0.06 * inch", "cSinkDiameter" : "0.138 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.0469 * inch", "holeDiameter" : "0.064 * inch", "cBoreDiameter" : "1/8 * inch", "cBoreDepth" : "0.06 * inch", "cSinkDiameter" : "0.138 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.052 * inch", "holeDiameter" : "0.07 * inch", "cBoreDiameter" : "1/8 * inch", "cBoreDepth" : "0.06 * inch", "cSinkDiameter" : "0.138 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.0469 * inch", "holeDiameter" : "0.07 * inch", "cBoreDiameter" : "1/8 * inch", "cBoreDepth" : "0.06 * inch", "cSinkDiameter" : "0.138 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "#1" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "64 tpi",
            "entries" : {
                "64 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.0625 * inch", "holeDiameter" : "0.076 * inch", "cBoreDiameter" : "5/32 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.0595 * inch", "holeDiameter" : "0.076 * inch", "cBoreDiameter" : "5/32 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.0625 * inch", "holeDiameter" : "0.081 * inch", "cBoreDiameter" : "5/32 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.0595 * inch", "holeDiameter" : "0.081 * inch", "cBoreDiameter" : "5/32 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "72 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.0635 * inch", "holeDiameter" : "0.076 * inch", "cBoreDiameter" : "5/33 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.0595 * inch", "holeDiameter" : "0.076 * inch", "cBoreDiameter" : "5/33 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.0635 * inch", "holeDiameter" : "0.081 * inch", "cBoreDiameter" : "5/33 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.0595 * inch", "holeDiameter" : "0.081 * inch", "cBoreDiameter" : "5/33 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "#2" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "56 tpi",
            "entries" : {
                "56 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.073 * inch", "holeDiameter" : "0.089 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.07 * inch", "holeDiameter" : "0.089 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.073 * inch", "holeDiameter" : "0.096 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.07 * inch", "holeDiameter" : "0.096 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "64 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.076 * inch", "holeDiameter" : "0.089 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.07 * inch", "holeDiameter" : "0.089 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.076 * inch", "holeDiameter" : "0.096 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.07 * inch", "holeDiameter" : "0.096 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "#3" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "48 tpi",
            "entries" : {
                "48 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.086 * inch", "holeDiameter" : "0.104 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.0785 * inch", "holeDiameter" : "0.104 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.086 * inch", "holeDiameter" : "0.11 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.0785 * inch", "holeDiameter" : "0.11 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "56 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.089 * inch", "holeDiameter" : "0.104 * inch", "cBoreDiameter" : "7/33 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.082 * inch", "holeDiameter" : "0.104 * inch", "cBoreDiameter" : "7/33 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.089 * inch", "holeDiameter" : "0.11 * inch", "cBoreDiameter" : "7/33 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.082 * inch", "holeDiameter" : "0.11 * inch", "cBoreDiameter" : "7/33 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "#4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "40 tpi",
            "entries" : {
                "40 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.096 * inch", "holeDiameter" : "0.116 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.089 * inch", "holeDiameter" : "0.116 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.096 * inch", "holeDiameter" : "0.1285 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.089 * inch", "holeDiameter" : "0.1285 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "48 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.098 * inch", "holeDiameter" : "0.116 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.0935 * inch", "holeDiameter" : "0.116 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.098 * inch", "holeDiameter" : "0.1285 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.0935 * inch", "holeDiameter" : "0.1285 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "#5" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "40 tpi",
            "entries" : {
                "40 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.1094 * inch", "holeDiameter" : "0.129 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.1015 * inch", "holeDiameter" : "0.129 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.1094 * inch", "holeDiameter" : "0.136 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.1015 * inch", "holeDiameter" : "0.136 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "44 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.11 * inch", "holeDiameter" : "0.129 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.104 * inch", "holeDiameter" : "0.129 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.11 * inch", "holeDiameter" : "0.136 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.104 * inch", "holeDiameter" : "0.136 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "#6" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "32 tpi",
            "entries" : {
                "32 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.116 * inch", "holeDiameter" : "0.144 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.1065 * inch", "holeDiameter" : "0.144 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.116 * inch", "holeDiameter" : "0.1495 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.1065 * inch", "holeDiameter" : "0.1495 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "40 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.12 * inch", "holeDiameter" : "0.144 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.113 * inch", "holeDiameter" : "0.144 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.12 * inch", "holeDiameter" : "0.1495 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.113 * inch", "holeDiameter" : "0.1495 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "#8" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "32 tpi",
            "entries" : {
                "32 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.144 * inch", "holeDiameter" : "0.170 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.136 * inch", "holeDiameter" : "0.170 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.144 * inch", "holeDiameter" : "0.177 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.136 * inch", "holeDiameter" : "0.177 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "36 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.147 * inch", "holeDiameter" : "0.170 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.136 * inch", "holeDiameter" : "0.170 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.147 * inch", "holeDiameter" : "0.177 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.136 * inch", "holeDiameter" : "0.177 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "#10" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "24 tpi",
            "entries" : {
                "24 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.161 * inch", "holeDiameter" : "0.196 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.1495 * inch", "holeDiameter" : "0.196 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.161 * inch", "holeDiameter" : "0.201 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.1495 * inch", "holeDiameter" : "0.201 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "32 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.1695 * inch", "holeDiameter" : "0.196 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.159 * inch", "holeDiameter" : "0.196 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.1695 * inch", "holeDiameter" : "0.201 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.159 * inch", "holeDiameter" : "0.201 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "1/4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "28 tpi",
            "entries" : {
                "20 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.2188 * inch", "holeDiameter" : "0.257 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.201 * inch", "holeDiameter" : "0.257 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.2188 * inch", "holeDiameter" : "0.266 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.201 * inch", "holeDiameter" : "0.266 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "28 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.228 * inch", "holeDiameter" : "0.257 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.213 * inch", "holeDiameter" : "0.257 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.228 * inch", "holeDiameter" : "0.266 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.213 * inch", "holeDiameter" : "0.266 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "32 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.228 * inch", "holeDiameter" : "0.257 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.2188 * inch", "holeDiameter" : "0.257 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.228 * inch", "holeDiameter" : "0.266 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.2188 * inch", "holeDiameter" : "0.266 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "5/16" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "24 tpi",
            "entries" : {
                "18 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.277 * inch", "holeDiameter" : "0.323 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.257 * inch", "holeDiameter" : "0.323 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.277 * inch", "holeDiameter" : "0.332 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.257 * inch", "holeDiameter" : "0.332 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "24 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.2812 * inch", "holeDiameter" : "0.323 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.272 * inch", "holeDiameter" : "0.323 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.2812 * inch", "holeDiameter" : "0.332 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.272 * inch", "holeDiameter" : "0.332 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "32 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.29 * inch", "holeDiameter" : "0.323 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.2812 * inch", "holeDiameter" : "0.323 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.29 * inch", "holeDiameter" : "0.332 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.2812 * inch", "holeDiameter" : "0.332 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "3/8" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "24 tpi",
            "entries" : {
                "16 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.332 * inch", "holeDiameter" : "0.386 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.3125 * inch", "holeDiameter" : "0.386 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.332 * inch", "holeDiameter" : "0.397 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.3125 * inch", "holeDiameter" : "0.397 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "24 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.348 * inch", "holeDiameter" : "0.386 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.332 * inch", "holeDiameter" : "0.386 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.348 * inch", "holeDiameter" : "0.397 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.332 * inch", "holeDiameter" : "0.397 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "32 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.358 * inch", "holeDiameter" : "0.386 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.3438 * inch", "holeDiameter" : "0.386 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.358 * inch", "holeDiameter" : "0.397 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.3438 * inch", "holeDiameter" : "0.397 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "7/16" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "20 tpi",
            "entries" : {
                "14 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.3906 * inch", "holeDiameter" : "0.453 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.368 * inch", "holeDiameter" : "0.453 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.3906 * inch", "holeDiameter" : "0.4687 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.368 * inch", "holeDiameter" : "0.4687 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "20 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4062 * inch", "holeDiameter" : "0.453 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.3906 * inch", "holeDiameter" : "0.453 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4062 * inch", "holeDiameter" : "0.4687 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.3906 * inch", "holeDiameter" : "0.4687 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "28 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.413 * inch", "holeDiameter" : "0.453 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.404 * inch", "holeDiameter" : "0.453 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.413 * inch", "holeDiameter" : "0.4687 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.404 * inch", "holeDiameter" : "0.4687 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "1/2" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "20 tpi",
            "entries" : {
                "13 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4531 * inch", "holeDiameter" : "0.516 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.4219 * inch", "holeDiameter" : "0.516 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4531 * inch", "holeDiameter" : "0.5312 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.4219 * inch", "holeDiameter" : "0.5312 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "20 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4688 * inch", "holeDiameter" : "0.516 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.4531 * inch", "holeDiameter" : "0.516 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4688 * inch", "holeDiameter" : "0.5312 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.4531 * inch", "holeDiameter" : "0.5312 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "28 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4688 * inch", "holeDiameter" : "0.516 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.4688 * inch", "holeDiameter" : "0.516 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4688 * inch", "holeDiameter" : "0.5312 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.4688 * inch", "holeDiameter" : "0.5312 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "5/8" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "18 tpi",
            "entries" : {
                "11 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5625 * inch", "holeDiameter" : "0.641 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.5312 * inch", "holeDiameter" : "0.641 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5625 * inch", "holeDiameter" : "0.6562 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.5312 * inch", "holeDiameter" : "0.6562 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "18 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5938 * inch", "holeDiameter" : "0.641 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.5781 * inch", "holeDiameter" : "0.641 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5938 * inch", "holeDiameter" : "0.6562 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.5781 * inch", "holeDiameter" : "0.6562 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "24 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5938 * inch", "holeDiameter" : "0.641 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.5781 * inch", "holeDiameter" : "0.641 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5938 * inch", "holeDiameter" : "0.6562 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.5781 * inch", "holeDiameter" : "0.6562 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "3/4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "16 tpi",
            "entries" : {
                "10 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.6875 * inch", "holeDiameter" : "0.766 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.6562 * inch", "holeDiameter" : "0.766 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.6875 * inch", "holeDiameter" : "0.7812 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.6562 * inch", "holeDiameter" : "0.7812 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "16 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7031 * inch", "holeDiameter" : "0.766 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.6875 * inch", "holeDiameter" : "0.766 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7031 * inch", "holeDiameter" : "0.7812 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.6875 * inch", "holeDiameter" : "0.7812 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "20 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7188 * inch", "holeDiameter" : "0.766 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.7031 * inch", "holeDiameter" : "0.766 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7188 * inch", "holeDiameter" : "0.7812 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.7031 * inch", "holeDiameter" : "0.7812 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "7/8" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "14 tpi",
            "entries" : {
                "9 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7969 * inch", "holeDiameter" : "0.891 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.7656 * inch", "holeDiameter" : "0.891 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7969 * inch", "holeDiameter" : "0.9062 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.7656 * inch", "holeDiameter" : "0.9062 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "14 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.8281 * inch", "holeDiameter" : "0.891 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.8125 * inch", "holeDiameter" : "0.891 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.8281 * inch", "holeDiameter" : "0.9062 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.8125 * inch", "holeDiameter" : "0.9062 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "20 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.8438 * inch", "holeDiameter" : "0.891 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.8281 * inch", "holeDiameter" : "0.891 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.8438 * inch", "holeDiameter" : "0.9062 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.8281 * inch", "holeDiameter" : "0.9062 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "1" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "12 tpi",
            "entries" : {
                "8 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9219 * inch", "holeDiameter" : "1.016 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.875 * inch", "holeDiameter" : "1.016 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9219 * inch", "holeDiameter" : "1.0313 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.875 * inch", "holeDiameter" : "1.0313 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "12 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9531 * inch", "holeDiameter" : "1.016 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.9219 * inch", "holeDiameter" : "1.016 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9531 * inch", "holeDiameter" : "1.0313 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.9219 * inch", "holeDiameter" : "1.0313 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "20 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9688 * inch", "holeDiameter" : "1.016 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.9531 * inch", "holeDiameter" : "1.016 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9688 * inch", "holeDiameter" : "1.0313 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "0.9531 * inch", "holeDiameter" : "1.0313 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "1 1/4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "12 tpi",
            "entries" : {
                "7 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.1562 * inch", "holeDiameter" : "1.2656 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "1.1094 * inch", "holeDiameter" : "1.2656 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.1562 * inch", "holeDiameter" : "1.2188 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "1.1094 * inch", "holeDiameter" : "1.2188 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "12 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2031 * inch", "holeDiameter" : "1.2656 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "1.1719 * inch", "holeDiameter" : "1.2656 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2031 * inch", "holeDiameter" : "1.2188 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "1.1719 * inch", "holeDiameter" : "1.2188 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "18 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2187 * inch", "holeDiameter" : "1.2656 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "1.1875 * inch", "holeDiameter" : "1.2656 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2187 * inch", "holeDiameter" : "1.2188 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "1.1875 * inch", "holeDiameter" : "1.2188 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "1 1/2" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "12 tpi",
            "entries" : {
                "6 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.3906 * inch", "holeDiameter" : "1.5156 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "1.3437 * inch", "holeDiameter" : "1.5156 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.3906 * inch", "holeDiameter" : "1.5312 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "1.3437 * inch", "holeDiameter" : "1.5312 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "12 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.4375 * inch", "holeDiameter" : "1.5156 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "1.4219 * inch", "holeDiameter" : "1.5156 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.4375 * inch", "holeDiameter" : "1.5312 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "1.4219 * inch", "holeDiameter" : "1.5312 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                },
                "18 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.4687 * inch", "holeDiameter" : "1.5156 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "1.4375 * inch", "holeDiameter" : "1.5156 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.4687 * inch", "holeDiameter" : "1.5312 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree"},
                                "75%" : {"tapDrillDiameter" : "1.4375 * inch", "holeDiameter" : "1.5312 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree"}
                            }
                        }
                    }
                }
            }
        }
    }
};

const ISO_drillTable = {
    "name" : "size",
    "displayName" : "Drill size",
    "default" : "5",
    "entries" : {
        "0.05" : {"holeDiameter" : "0.05 * millimeter", "tapDrillDiameter" : "0.05 * millimeter"},
        "0.1" : {"holeDiameter" : "0.1 * millimeter", "tapDrillDiameter" : "0.1 * millimeter"},
        "0.2" : {"holeDiameter" : "0.2 * millimeter", "tapDrillDiameter" : "0.2 * millimeter"},
        "0.3" : {"holeDiameter" : "0.3 * millimeter", "tapDrillDiameter" : "0.3 * millimeter"},
        "0.4" : {"holeDiameter" : "0.4 * millimeter", "tapDrillDiameter" : "0.4 * millimeter"},
        "0.5" : {"holeDiameter" : "0.5 * millimeter", "tapDrillDiameter" : "0.5 * millimeter"},
        "0.6" : {"holeDiameter" : "0.6 * millimeter", "tapDrillDiameter" : "0.6 * millimeter"},
        "0.7" : {"holeDiameter" : "0.7 * millimeter", "tapDrillDiameter" : "0.7 * millimeter"},
        "0.8" : {"holeDiameter" : "0.8 * millimeter", "tapDrillDiameter" : "0.8 * millimeter"},
        "0.9" : {"holeDiameter" : "0.9 * millimeter", "tapDrillDiameter" : "0.9 * millimeter"},
        "1" : {"holeDiameter" : "1 * millimeter", "tapDrillDiameter" : "1 * millimeter"},
        "1.1" : {"holeDiameter" : "1.1 * millimeter", "tapDrillDiameter" : "1.1 * millimeter"},
        "1.2" : {"holeDiameter" : "1.2 * millimeter", "tapDrillDiameter" : "1.2 * millimeter"},
        "1.3" : {"holeDiameter" : "1.3 * millimeter", "tapDrillDiameter" : "1.3 * millimeter"},
        "1.4" : {"holeDiameter" : "1.4 * millimeter", "tapDrillDiameter" : "1.4 * millimeter"},
        "1.5" : {"holeDiameter" : "1.5 * millimeter", "tapDrillDiameter" : "1.5 * millimeter"},
        "1.6" : {"holeDiameter" : "1.6 * millimeter", "tapDrillDiameter" : "1.6 * millimeter"},
        "1.7" : {"holeDiameter" : "1.7 * millimeter", "tapDrillDiameter" : "1.7 * millimeter"},
        "1.8" : {"holeDiameter" : "1.8 * millimeter", "tapDrillDiameter" : "1.8 * millimeter"},
        "1.9" : {"holeDiameter" : "1.9 * millimeter", "tapDrillDiameter" : "1.9 * millimeter"},
        "2" : {"holeDiameter" : "2 * millimeter", "tapDrillDiameter" : "2 * millimeter"},
        "2.1" : {"holeDiameter" : "2.1 * millimeter", "tapDrillDiameter" : "2.1 * millimeter"},
        "2.2" : {"holeDiameter" : "2.2 * millimeter", "tapDrillDiameter" : "2.2 * millimeter"},
        "2.3" : {"holeDiameter" : "2.3 * millimeter", "tapDrillDiameter" : "2.3 * millimeter"},
        "2.4" : {"holeDiameter" : "2.4 * millimeter", "tapDrillDiameter" : "2.4 * millimeter"},
        "2.5" : {"holeDiameter" : "2.5 * millimeter", "tapDrillDiameter" : "2.5 * millimeter"},
        "2.6" : {"holeDiameter" : "2.6 * millimeter", "tapDrillDiameter" : "2.6 * millimeter"},
        "2.7" : {"holeDiameter" : "2.7 * millimeter", "tapDrillDiameter" : "2.7 * millimeter"},
        "2.8" : {"holeDiameter" : "2.8 * millimeter", "tapDrillDiameter" : "2.8 * millimeter"},
        "2.9" : {"holeDiameter" : "2.9 * millimeter", "tapDrillDiameter" : "2.9 * millimeter"},
        "3" : {"holeDiameter" : "3 * millimeter", "tapDrillDiameter" : "3 * millimeter"},
        "3.1" : {"holeDiameter" : "3.1 * millimeter", "tapDrillDiameter" : "3.1 * millimeter"},
        "3.2" : {"holeDiameter" : "3.2 * millimeter", "tapDrillDiameter" : "3.2 * millimeter"},
        "3.3" : {"holeDiameter" : "3.3 * millimeter", "tapDrillDiameter" : "3.3 * millimeter"},
        "3.4" : {"holeDiameter" : "3.4 * millimeter", "tapDrillDiameter" : "3.4 * millimeter"},
        "3.5" : {"holeDiameter" : "3.5 * millimeter", "tapDrillDiameter" : "3.5 * millimeter"},
        "3.6" : {"holeDiameter" : "3.6 * millimeter", "tapDrillDiameter" : "3.6 * millimeter"},
        "3.7" : {"holeDiameter" : "3.7 * millimeter", "tapDrillDiameter" : "3.7 * millimeter"},
        "3.8" : {"holeDiameter" : "3.8 * millimeter", "tapDrillDiameter" : "3.8 * millimeter"},
        "3.9" : {"holeDiameter" : "3.9 * millimeter", "tapDrillDiameter" : "3.9 * millimeter"},
        "4" : {"holeDiameter" : "4 * millimeter", "tapDrillDiameter" : "4 * millimeter"},
        "4.1" : {"holeDiameter" : "4.1 * millimeter", "tapDrillDiameter" : "4.1 * millimeter"},
        "4.2" : {"holeDiameter" : "4.2 * millimeter", "tapDrillDiameter" : "4.2 * millimeter"},
        "4.3" : {"holeDiameter" : "4.3 * millimeter", "tapDrillDiameter" : "4.3 * millimeter"},
        "4.4" : {"holeDiameter" : "4.4 * millimeter", "tapDrillDiameter" : "4.4 * millimeter"},
        "4.5" : {"holeDiameter" : "4.5 * millimeter", "tapDrillDiameter" : "4.5 * millimeter"},
        "4.6" : {"holeDiameter" : "4.6 * millimeter", "tapDrillDiameter" : "4.6 * millimeter"},
        "4.7" : {"holeDiameter" : "4.7 * millimeter", "tapDrillDiameter" : "4.7 * millimeter"},
        "4.8" : {"holeDiameter" : "4.8 * millimeter", "tapDrillDiameter" : "4.8 * millimeter"},
        "4.9" : {"holeDiameter" : "4.9 * millimeter", "tapDrillDiameter" : "4.9 * millimeter"},
        "5" : {"holeDiameter" : "5 * millimeter", "tapDrillDiameter" : "5 * millimeter"},
        "5.1" : {"holeDiameter" : "5.1 * millimeter", "tapDrillDiameter" : "5.1 * millimeter"},
        "5.2" : {"holeDiameter" : "5.2 * millimeter", "tapDrillDiameter" : "5.2 * millimeter"},
        "5.3" : {"holeDiameter" : "5.3 * millimeter", "tapDrillDiameter" : "5.3 * millimeter"},
        "5.4" : {"holeDiameter" : "5.4 * millimeter", "tapDrillDiameter" : "5.4 * millimeter"},
        "5.5" : {"holeDiameter" : "5.5 * millimeter", "tapDrillDiameter" : "5.5 * millimeter"},
        "5.6" : {"holeDiameter" : "5.6 * millimeter", "tapDrillDiameter" : "5.6 * millimeter"},
        "5.7" : {"holeDiameter" : "5.7 * millimeter", "tapDrillDiameter" : "5.7 * millimeter"},
        "5.8" : {"holeDiameter" : "5.8 * millimeter", "tapDrillDiameter" : "5.8 * millimeter"},
        "5.9" : {"holeDiameter" : "5.9 * millimeter", "tapDrillDiameter" : "5.9 * millimeter"},
        "6" : {"holeDiameter" : "6 * millimeter", "tapDrillDiameter" : "6 * millimeter"},
        "6.1" : {"holeDiameter" : "6.1 * millimeter", "tapDrillDiameter" : "6.1 * millimeter"},
        "6.2" : {"holeDiameter" : "6.2 * millimeter", "tapDrillDiameter" : "6.2 * millimeter"},
        "6.3" : {"holeDiameter" : "6.3 * millimeter", "tapDrillDiameter" : "6.3 * millimeter"},
        "6.4" : {"holeDiameter" : "6.4 * millimeter", "tapDrillDiameter" : "6.4 * millimeter"},
        "6.5" : {"holeDiameter" : "6.5 * millimeter", "tapDrillDiameter" : "6.5 * millimeter"},
        "6.6" : {"holeDiameter" : "6.6 * millimeter", "tapDrillDiameter" : "6.6 * millimeter"},
        "6.7" : {"holeDiameter" : "6.7 * millimeter", "tapDrillDiameter" : "6.7 * millimeter"},
        "6.8" : {"holeDiameter" : "6.8 * millimeter", "tapDrillDiameter" : "6.8 * millimeter"},
        "6.9" : {"holeDiameter" : "6.9 * millimeter", "tapDrillDiameter" : "6.9 * millimeter"},
        "7" : {"holeDiameter" : "7 * millimeter", "tapDrillDiameter" : "7 * millimeter"},
        "7.1" : {"holeDiameter" : "7.1 * millimeter", "tapDrillDiameter" : "7.1 * millimeter"},
        "7.2" : {"holeDiameter" : "7.2 * millimeter", "tapDrillDiameter" : "7.2 * millimeter"},
        "7.3" : {"holeDiameter" : "7.3 * millimeter", "tapDrillDiameter" : "7.3 * millimeter"},
        "7.4" : {"holeDiameter" : "7.4 * millimeter", "tapDrillDiameter" : "7.4 * millimeter"},
        "7.5" : {"holeDiameter" : "7.5 * millimeter", "tapDrillDiameter" : "7.5 * millimeter"},
        "7.6" : {"holeDiameter" : "7.6 * millimeter", "tapDrillDiameter" : "7.6 * millimeter"},
        "7.7" : {"holeDiameter" : "7.7 * millimeter", "tapDrillDiameter" : "7.7 * millimeter"},
        "7.8" : {"holeDiameter" : "7.8 * millimeter", "tapDrillDiameter" : "7.8 * millimeter"},
        "7.9" : {"holeDiameter" : "7.9 * millimeter", "tapDrillDiameter" : "7.9 * millimeter"},
        "8" : {"holeDiameter" : "8 * millimeter", "tapDrillDiameter" : "8 * millimeter"},
        "8.1" : {"holeDiameter" : "8.1 * millimeter", "tapDrillDiameter" : "8.1 * millimeter"},
        "8.2" : {"holeDiameter" : "8.2 * millimeter", "tapDrillDiameter" : "8.2 * millimeter"},
        "8.3" : {"holeDiameter" : "8.3 * millimeter", "tapDrillDiameter" : "8.3 * millimeter"},
        "8.4" : {"holeDiameter" : "8.4 * millimeter", "tapDrillDiameter" : "8.4 * millimeter"},
        "8.5" : {"holeDiameter" : "8.5 * millimeter", "tapDrillDiameter" : "8.5 * millimeter"},
        "8.6" : {"holeDiameter" : "8.6 * millimeter", "tapDrillDiameter" : "8.6 * millimeter"},
        "8.7" : {"holeDiameter" : "8.7 * millimeter", "tapDrillDiameter" : "8.7 * millimeter"},
        "8.8" : {"holeDiameter" : "8.8 * millimeter", "tapDrillDiameter" : "8.8 * millimeter"},
        "8.9" : {"holeDiameter" : "8.9 * millimeter", "tapDrillDiameter" : "8.9 * millimeter"},
        "9" : {"holeDiameter" : "9 * millimeter", "tapDrillDiameter" : "9 * millimeter"},
        "9.1" : {"holeDiameter" : "9.1 * millimeter", "tapDrillDiameter" : "9.1 * millimeter"},
        "9.2" : {"holeDiameter" : "9.2 * millimeter", "tapDrillDiameter" : "9.2 * millimeter"},
        "9.3" : {"holeDiameter" : "9.3 * millimeter", "tapDrillDiameter" : "9.3 * millimeter"},
        "9.4" : {"holeDiameter" : "9.4 * millimeter", "tapDrillDiameter" : "9.4 * millimeter"},
        "9.5" : {"holeDiameter" : "9.5 * millimeter", "tapDrillDiameter" : "9.5 * millimeter"},
        "9.6" : {"holeDiameter" : "9.6 * millimeter", "tapDrillDiameter" : "9.6 * millimeter"},
        "9.7" : {"holeDiameter" : "9.7 * millimeter", "tapDrillDiameter" : "9.7 * millimeter"},
        "9.8" : {"holeDiameter" : "9.8 * millimeter", "tapDrillDiameter" : "9.8 * millimeter"},
        "9.9" : {"holeDiameter" : "9.9 * millimeter", "tapDrillDiameter" : "9.9 * millimeter"},
        "10" : {"holeDiameter" : "10 * millimeter", "tapDrillDiameter" : "10 * millimeter"},
        "10.5" : {"holeDiameter" : "10.5 * millimeter", "tapDrillDiameter" : "10.5 * millimeter"},
        "11" : {"holeDiameter" : "11 * millimeter", "tapDrillDiameter" : "11 * millimeter"},
        "11.5" : {"holeDiameter" : "11.5 * millimeter", "tapDrillDiameter" : "11.5 * millimeter"},
        "12" : {"holeDiameter" : "12 * millimeter", "tapDrillDiameter" : "12 * millimeter"},
        "12.5" : {"holeDiameter" : "12.5 * millimeter", "tapDrillDiameter" : "12.5 * millimeter"},
        "13" : {"holeDiameter" : "13 * millimeter", "tapDrillDiameter" : "13 * millimeter"},
        "13.5" : {"holeDiameter" : "13.5 * millimeter", "tapDrillDiameter" : "13.5 * millimeter"},
        "14" : {"holeDiameter" : "14 * millimeter", "tapDrillDiameter" : "14 * millimeter"},
        "14.5" : {"holeDiameter" : "14.5 * millimeter", "tapDrillDiameter" : "14.5 * millimeter"},
        "15" : {"holeDiameter" : "15 * millimeter", "tapDrillDiameter" : "15 * millimeter"},
        "15.5" : {"holeDiameter" : "15.5 * millimeter", "tapDrillDiameter" : "15.5 * millimeter"},
        "16" : {"holeDiameter" : "16 * millimeter", "tapDrillDiameter" : "16 * millimeter"},
        "16.5" : {"holeDiameter" : "16.5 * millimeter", "tapDrillDiameter" : "16.5 * millimeter"},
        "17" : {"holeDiameter" : "17 * millimeter", "tapDrillDiameter" : "17 * millimeter"},
        "17.5" : {"holeDiameter" : "17.5 * millimeter", "tapDrillDiameter" : "17.5 * millimeter"},
        "18" : {"holeDiameter" : "18 * millimeter", "tapDrillDiameter" : "18 * millimeter"},
        "18.5" : {"holeDiameter" : "18.5 * millimeter", "tapDrillDiameter" : "18.5 * millimeter"},
        "19" : {"holeDiameter" : "19 * millimeter", "tapDrillDiameter" : "19 * millimeter"},
        "19.5" : {"holeDiameter" : "19.5 * millimeter", "tapDrillDiameter" : "19.5 * millimeter"},
        "20" : {"holeDiameter" : "20 * millimeter", "tapDrillDiameter" : "20 * millimeter"},
        "20.5" : {"holeDiameter" : "20.5 * millimeter", "tapDrillDiameter" : "20.5 * millimeter"},
        "21" : {"holeDiameter" : "21 * millimeter", "tapDrillDiameter" : "21 * millimeter"},
        "21.5" : {"holeDiameter" : "21.5 * millimeter", "tapDrillDiameter" : "21.5 * millimeter"},
        "22" : {"holeDiameter" : "22 * millimeter", "tapDrillDiameter" : "22 * millimeter"},
        "22.5" : {"holeDiameter" : "22.5 * millimeter", "tapDrillDiameter" : "22.5 * millimeter"},
        "23" : {"holeDiameter" : "23 * millimeter", "tapDrillDiameter" : "23 * millimeter"},
        "23.5" : {"holeDiameter" : "23.5 * millimeter", "tapDrillDiameter" : "23.5 * millimeter"},
        "24" : {"holeDiameter" : "24 * millimeter", "tapDrillDiameter" : "24 * millimeter"},
        "24.5" : {"holeDiameter" : "24.5 * millimeter", "tapDrillDiameter" : "24.5 * millimeter"},
        "25" : {"holeDiameter" : "25 * millimeter", "tapDrillDiameter" : "25 * millimeter"},
        "25.5" : {"holeDiameter" : "25.5 * millimeter", "tapDrillDiameter" : "25.5 * millimeter"},
        "26" : {"holeDiameter" : "26 * millimeter", "tapDrillDiameter" : "26 * millimeter"},
        "26.5" : {"holeDiameter" : "26.5 * millimeter", "tapDrillDiameter" : "26.5 * millimeter"},
        "27" : {"holeDiameter" : "27 * millimeter", "tapDrillDiameter" : "27 * millimeter"},
        "27.5" : {"holeDiameter" : "27.5 * millimeter", "tapDrillDiameter" : "27.5 * millimeter"},
        "28" : {"holeDiameter" : "28 * millimeter", "tapDrillDiameter" : "28 * millimeter"},
        "28.5" : {"holeDiameter" : "28.5 * millimeter", "tapDrillDiameter" : "28.5 * millimeter"},
        "29" : {"holeDiameter" : "29 * millimeter", "tapDrillDiameter" : "29 * millimeter"},
        "29.5" : {"holeDiameter" : "29.5 * millimeter", "tapDrillDiameter" : "29.5 * millimeter"},
        "30" : {"holeDiameter" : "30 * millimeter", "tapDrillDiameter" : "30 * millimeter"},
        "30.5" : {"holeDiameter" : "30.5 * millimeter", "tapDrillDiameter" : "30.5 * millimeter"},
        "31" : {"holeDiameter" : "31 * millimeter", "tapDrillDiameter" : "31 * millimeter"},
        "31.5" : {"holeDiameter" : "31.5 * millimeter", "tapDrillDiameter" : "31.5 * millimeter"},
        "32" : {"holeDiameter" : "32 * millimeter", "tapDrillDiameter" : "32 * millimeter"},
        "32.5" : {"holeDiameter" : "32.5 * millimeter", "tapDrillDiameter" : "32.5 * millimeter"},
        "33" : {"holeDiameter" : "33 * millimeter", "tapDrillDiameter" : "33 * millimeter"},
        "33.5" : {"holeDiameter" : "33.5 * millimeter", "tapDrillDiameter" : "33.5 * millimeter"},
        "34" : {"holeDiameter" : "34 * millimeter", "tapDrillDiameter" : "34 * millimeter"},
        "34.5" : {"holeDiameter" : "34.5 * millimeter", "tapDrillDiameter" : "34.5 * millimeter"},
        "35" : {"holeDiameter" : "35 * millimeter", "tapDrillDiameter" : "35 * millimeter"},
        "35.5" : {"holeDiameter" : "35.5 * millimeter", "tapDrillDiameter" : "35.5 * millimeter"},
        "36" : {"holeDiameter" : "36 * millimeter", "tapDrillDiameter" : "36 * millimeter"},
        "36.5" : {"holeDiameter" : "36.5 * millimeter", "tapDrillDiameter" : "36.5 * millimeter"},
        "37" : {"holeDiameter" : "37 * millimeter", "tapDrillDiameter" : "37 * millimeter"},
        "37.5" : {"holeDiameter" : "37.5 * millimeter", "tapDrillDiameter" : "37.5 * millimeter"},
        "38" : {"holeDiameter" : "38 * millimeter", "tapDrillDiameter" : "38 * millimeter"}
    }
};

const ISO_ClearanceHoleTable = {
    "name" : "size",
    "displayName" : "Size",
    "default" : "M5",
    "entries" : {
        "M3" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Standard",
            "entries" : {
                "Close" : {"cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.15 * millimeter", "cSinkAngle" : "90 * degree"},
                "Standard" : {"cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.3 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Standard",
            "entries" : {
                "Close" : {"cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.2 * millimeter", "cSinkAngle" : "90 * degree"},
                "Standard" : {"cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.4 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M5" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Standard",
            "entries" : {
                "Close" : {"cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.25 * millimeter", "cSinkAngle" : "90 * degree"},
                "Standard" : {"cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.5 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M6" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Standard",
            "entries" : {
                "Close" : {"cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "6.3 * millimeter", "cSinkAngle" : "90 * degree"},
                "Standard" : {"cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "6.6 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M8" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Standard",
            "entries" : {
                "Close" : {"cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "8.4 * millimeter", "cSinkAngle" : "90 * degree"},
                "Standard" : {"cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "8.8 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M10" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Standard",
            "entries" : {
                "Close" : {"cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "10.5 * millimeter", "cSinkAngle" : "90 * degree"},
                "Standard" : {"cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "11 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M12" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Standard",
            "entries" : {
                "Close" : {"cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "12.6 * millimeter", "cSinkAngle" : "90 * degree"},
                "Standard" : {"cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "13.2 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M14" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Standard",
            "entries" : {
                "Close" : {"cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "14.75 * millimeter", "cSinkAngle" : "90 * degree"},
                "Standard" : {"cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "15.5 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M16" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Standard",
            "entries" : {
                "Close" : {"cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "16.75 * millimeter", "cSinkAngle" : "90 * degree"},
                "Standard" : {"cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "17.5 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M20" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Standard",
            "entries" : {
                "Close" : {"cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "21 * millimeter", "cSinkAngle" : "90 * degree"},
                "Standard" : {"cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "22 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        }
    }
};

const ISO_TappedHoleTable = {
    "name" : "size",
    "displayName" : "Size",
    "default" : "M5",
    "entries" : {
        "M3" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.60 mm",
            "entries" : {
                "0.50 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "2.7 * millimeter", "tapDrillDiameter" : "2.7 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "2.5 * millimeter", "tapDrillDiameter" : "2.5 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                },
                "0.60 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "2.6 * millimeter", "tapDrillDiameter" : "2.6 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "2.4 * millimeter", "tapDrillDiameter" : "2.4 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                }
            }
        },
        "M4" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.75 mm",
            "entries" : {
                "0.70 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "3.5 * millimeter", "tapDrillDiameter" : "3.5 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "3.3 * millimeter", "tapDrillDiameter" : "3.3 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                },
                "0.75 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "3.5 * millimeter", "tapDrillDiameter" : "3.5 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "3.2 * millimeter", "tapDrillDiameter" : "3.2 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                }
            }
        },
        "M5" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.90 mm",
            "entries" : {
                "0.80 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "4.5 * millimeter", "tapDrillDiameter" : "4.5 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "4.2 * millimeter", "tapDrillDiameter" : "4.2 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                },
                "0.90 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "4.4 * millimeter", "tapDrillDiameter" : "4.4 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "4.1 * millimeter", "tapDrillDiameter" : "4.1 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                },
                "1.00 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "4.4 * millimeter", "tapDrillDiameter" : "4.4 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "4 * millimeter", "tapDrillDiameter" : "4 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                }
            }
        },
        "M6" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "1.00 mm",
            "entries" : {
                "0.75 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "5.5 * millimeter", "tapDrillDiameter" : "5.5 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "5.25 * millimeter", "tapDrillDiameter" : "5.25 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                },
                "1.00 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "5.4 * millimeter", "tapDrillDiameter" : "5.4 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "5 * millimeter", "tapDrillDiameter" : "5 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                }
            }
        },
        "M8" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "1.25 mm",
            "entries" : {
                "1.00 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "7.4 * millimeter", "tapDrillDiameter" : "7.4 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "7 * millimeter", "tapDrillDiameter" : "7 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                },
                "1.25 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "7.2 * millimeter", "tapDrillDiameter" : "7.2 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "6.8 * millimeter", "tapDrillDiameter" : "6.8 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                }
            }
        },
        "M10" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "1.25 mm",
            "entries" : {
                "1.00 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "9.4 * millimeter", "tapDrillDiameter" : "9.4 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "9 * millimeter", "tapDrillDiameter" : "9 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                },
                "1.25 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "9.2 * millimeter", "tapDrillDiameter" : "9.2 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "8.8 * millimeter", "tapDrillDiameter" : "8.8 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                },
                "1.50 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "9 * millimeter", "tapDrillDiameter" : "9 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "8.5 * millimeter", "tapDrillDiameter" : "8.5 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                }
            }
        },
        "M12" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "1.50 mm",
            "entries" : {
                "1.25 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "11.2 * millimeter", "tapDrillDiameter" : "11.2 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "10.8 * millimeter", "tapDrillDiameter" : "10.8 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                },
                "1.50 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "11 * millimeter", "tapDrillDiameter" : "11 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "10.5 * millimeter", "tapDrillDiameter" : "10.5 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                },
                "1.75 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "11.2 * millimeter", "tapDrillDiameter" : "11.2 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "10.3 * millimeter", "tapDrillDiameter" : "10.3 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                }
            }
        },
        "M14" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "1.50 mm",
            "entries" : {
                "1.25 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "13.2 * millimeter", "tapDrillDiameter" : "13.2 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "12.8 * millimeter", "tapDrillDiameter" : "12.8 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                },
                "1.50 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "13 * millimeter", "tapDrillDiameter" : "13 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "12.5 * millimeter", "tapDrillDiameter" : "12.5 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                },
                "2.00 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "12.7 * millimeter", "tapDrillDiameter" : "12.7 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "12.1 * millimeter", "tapDrillDiameter" : "12.1 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                }
            }
        },
        "M16" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "2.00 mm",
            "entries" : {
                "1.50 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "15 * millimeter", "tapDrillDiameter" : "15 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "14.5 * millimeter", "tapDrillDiameter" : "14.5 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                },
                "2.00 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "14.75 * millimeter", "tapDrillDiameter" : "14.75 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "14 * millimeter", "tapDrillDiameter" : "14 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                }
            }
        },
        "M20" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "2.00 mm",
            "entries" : {
                "1.50 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "19 * millimeter", "tapDrillDiameter" : "19 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "18.5 * millimeter", "tapDrillDiameter" : "18.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                },
                "2.00 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "18.5 * millimeter", "tapDrillDiameter" : "18.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "18 * millimeter", "tapDrillDiameter" : "18 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                },
                "2.50 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "25%" : {"holeDiameter" : "18.5 * millimeter", "tapDrillDiameter" : "18.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "cSinkAngle" : "90 * degree"},
                        "75%" : {"holeDiameter" : "17.5 * millimeter", "tapDrillDiameter" : "17.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "cSinkAngle" : "90 * degree"}
                    }
                }
            }
        }
    }
};

const ISO_ThroughTappedScrewTable = {
    "name" : "size",
    "displayName" : "Size",
    "default" : "M5",
    "entries" : {
        "M3" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.60 mm",
            "entries" : {
                "0.50 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "2.7 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.15 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "2.5 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.15 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "2.7 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.3 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "2.5 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.3 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                },
                "0.60 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "2.6 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.15 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "2.4 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.15 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "2.6 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.3 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "2.4 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.3 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "M4" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.75 mm",
            "entries" : {
                "0.70 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "3.5 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.2 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "3.3 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.2 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "3.5 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.4 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "3.3 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.4 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                },
                "0.75 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "3.5 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.2 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "3.2 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.2 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "3.5 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.4 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "3.2 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.4 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "M5" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.90 mm",
            "entries" : {
                "0.80 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "4.5 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.25 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "4.2 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.25 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "4.5 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.5 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "4.2 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.5 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                },
                "0.90 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "4.4 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.25 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "4.1 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.25 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "4.4 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.5 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "4.1 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.5 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                },
                "1.00 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "4.4 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.25 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "4 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.25 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "4.4 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.5 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "4 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.5 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "M6" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "1.00 mm",
            "entries" : {
                "0.75 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "5.5 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "6.3 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "5.25 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "6.3 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "5.5 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "6.6 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "5.25 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "6.6 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                },
                "1.00 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "5.4 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "6.3 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "5 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "6.3 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "5.4 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "6.6 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "5 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "6.6 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "M8" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "1.25 mm",
            "entries" : {
                "1.00 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "7.4 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "8.4 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "7 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "8.4 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "7.4 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "8.8 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "7 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "8.8 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                },
                "1.25 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "7.2 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "8.4 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "6.8 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "8.4 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "7.2 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "8.8 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "6.8 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "8.8 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "M10" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "1.25 mm",
            "entries" : {
                "1.00 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "9.4 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "10.5 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "9 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "10.5 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "9.4 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "11 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "9 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "11 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                },
                "1.25 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "9.2 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "10.5 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "8.8 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "10.5 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "9.2 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "11 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "8.8 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "11 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                },
                "1.50 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "9 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "10.5 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "8.5 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "10.5 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "9 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "11 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "8.5 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "11 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "M12" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "1.50 mm",
            "entries" : {
                "1.25 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "11.2 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "12.6 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "10.8 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "12.6 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "11.2 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "13.2 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "10.8 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "13.2 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                },
                "1.50 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "11 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "12.6 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "10.5 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "12.6 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "11 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "13.2 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "10.5 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "13.2 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                },
                "1.75 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "11.2 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "12.6 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "10.3 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "12.6 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "11.2 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "13.2 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "10.3 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "13.2 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "M14" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "1.50 mm",
            "entries" : {
                "1.25 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "13.2 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "14.75 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "12.8 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "14.75 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "13.2 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "15.5 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "12.8 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "15.5 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                },
                "1.50 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "13 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "14.75 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "12.5 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "14.75 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "13 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "15.5 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "12.5 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "15.5 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                },
                "2.00 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "12.7 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "14.75 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "12.1 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "14.75 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "12.7 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "15.5 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "12.1 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "15.5 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "M16" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "2.00 mm",
            "entries" : {
                "1.50 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "15 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "16.75 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "14.5 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "16.75 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "15 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "17.5 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "14.5 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "17.5 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                },
                "2.00 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "14.75 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "16.75 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "14 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "16.75 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "14.75 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "17.5 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "14 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "17.5 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                }
            }
        },
        "M20" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "2.00 mm",
            "entries" : {
                "1.50 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "19 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "21 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "18.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "21 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "19 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "22 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "18.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "22 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                },
                "2.00 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "18.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "21 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "18 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "21 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "18.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "22 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "18 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "22 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                },
                "2.50 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Standard",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "18.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "21 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "17.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "21 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        },
                        "Standard" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "25%" : {"tapDrillDiameter" : "18.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "22 * millimeter", "cSinkAngle" : "90 * degree"},
                                "75%" : {"tapDrillDiameter" : "17.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "22 * millimeter", "cSinkAngle" : "90 * degree"}
                            }
                        }
                    }
                }
            }
        }
    }
};

const ANSI_ThroughScrewTable = {
    "name" : "type",
    "displayName" : "Hole type",
    "default" : "Clearance",
    "entries" : {
        "Clearance" : ANSI_ClearanceHoleTable,
        "Tapped" : ANSI_TappedHoleTable,
        "Drilled" : ANSI_drillTable
    }
};

const ISO_ThroughScrewTable = {
    "name" : "type",
    "displayName" : "Hole type",
    "default" : "Clearance",
    "entries" : {
        "Clearance" : ISO_ClearanceHoleTable,
        "Tapped" : ISO_TappedHoleTable,
        "Drilled" : ISO_drillTable
    }
};

/** @internal */
export const tappedOrClearanceHoleTable = {
    "name" : "standard",
    "displayName" : "Standard",
    "entries" : {
        "Custom" : {},
        "ANSI" : ANSI_ThroughScrewTable,
        "ISO" : ISO_ThroughScrewTable
    }
};

const ANSI_BlindInLastHoleTable = {
    "name" : "type",
    "displayName" : "Hole type",
    "default" : "Clearance & tapped",
    "entries" : {
        "Clearance & tapped" : ANSI_ThroughTappedScrewTable,
        "Drilled" : ANSI_drillTable
    }
};

const ISO_BlindInLastHoleTable = {
    "name" : "type",
    "displayName" : "Hole type",
    "default" : "Clearance & tapped",
    "entries" : {
        "Clearance & tapped" : ISO_ThroughTappedScrewTable,
        "Drilled" : ISO_drillTable
    }
};

/** @internal */
export const blindInLastHoleTable = {
    "name" : "standard",
    "displayName" : "Standard",
    "entries" : {
        "Custom" : {},
        "ANSI" : ANSI_BlindInLastHoleTable,
        "ISO" : ISO_BlindInLastHoleTable
    }
};


