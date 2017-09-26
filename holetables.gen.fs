FeatureScript 686; /* Automatically generated version */
/* Automatically generated file -- DO NOT EDIT */

import(path : "onshape/std/units.fs", version : "686.0");
import(path : "onshape/std/lookupTablePath.fs", version : "686.0");

const ANSI_drillTable = {
    "name" : "size",
    "displayName" : "Drill size",
    "default" : "1/4 (0.25)",
    "entries" : {
        "#80 (0.0135)" : {"holeDiameter" : "0.0135 in", "tapDrillDiameter" : "0.0135 in"},
        "#79 (0.0145)" : {"holeDiameter" : "0.0145 in", "tapDrillDiameter" : "0.0145 in"},
        "1/64 (0.0156)" : {"holeDiameter" : "1/64 in", "tapDrillDiameter" : "1/64 in"},
        "#78 (0.016)" : {"holeDiameter" : "0.016 in", "tapDrillDiameter" : "0.016 in"},
        "#77 (0.018)" : {"holeDiameter" : "0.018 in", "tapDrillDiameter" : "0.018 in"},
        "#76 (0.02)" : {"holeDiameter" : "0.02 in", "tapDrillDiameter" : "0.02 in"},
        "#75 (0.021)" : {"holeDiameter" : "0.021 in", "tapDrillDiameter" : "0.021 in"},
        "#74 (0.0225)" : {"holeDiameter" : "0.0225 in", "tapDrillDiameter" : "0.0225 in"},
        "#73 (0.024)" : {"holeDiameter" : "0.024 in", "tapDrillDiameter" : "0.024 in"},
        "#72 (0.025)" : {"holeDiameter" : "0.025 in", "tapDrillDiameter" : "0.025 in"},
        "#71 (0.026)" : {"holeDiameter" : "0.026 in", "tapDrillDiameter" : "0.026 in"},
        "#70 (0.028)" : {"holeDiameter" : "0.028 in", "tapDrillDiameter" : "0.028 in"},
        "#69 (0.0292)" : {"holeDiameter" : "0.0292 in", "tapDrillDiameter" : "0.0292 in"},
        "#68 (0.031)" : {"holeDiameter" : "0.031 in", "tapDrillDiameter" : "0.031 in"},
        "1/32 (0.0312)" : {"holeDiameter" : "1/32 in", "tapDrillDiameter" : "1/32 in"},
        "#67 (0.032)" : {"holeDiameter" : "0.032 in", "tapDrillDiameter" : "0.032 in"},
        "#66 (0.033)" : {"holeDiameter" : "0.033 in", "tapDrillDiameter" : "0.033 in"},
        "#65 (0.035)" : {"holeDiameter" : "0.035 in", "tapDrillDiameter" : "0.035 in"},
        "#64 (0.036)" : {"holeDiameter" : "0.036 in", "tapDrillDiameter" : "0.036 in"},
        "#63 (0.037)" : {"holeDiameter" : "0.037 in", "tapDrillDiameter" : "0.037 in"},
        "#62 (0.038)" : {"holeDiameter" : "0.038 in", "tapDrillDiameter" : "0.038 in"},
        "#61 (0.039)" : {"holeDiameter" : "0.039 in", "tapDrillDiameter" : "0.039 in"},
        "#60 (0.04)" : {"holeDiameter" : "0.04 in", "tapDrillDiameter" : "0.04 in"},
        "#59 (0.041)" : {"holeDiameter" : "0.041 in", "tapDrillDiameter" : "0.041 in"},
        "#58 (0.042)" : {"holeDiameter" : "0.042 in", "tapDrillDiameter" : "0.042 in"},
        "#57 (0.043)" : {"holeDiameter" : "0.043 in", "tapDrillDiameter" : "0.043 in"},
        "#56 (0.0465)" : {"holeDiameter" : "0.0465 in", "tapDrillDiameter" : "0.0465 in"},
        "3/64 (0.0469)" : {"holeDiameter" : "3/64 in", "tapDrillDiameter" : "3/64 in"},
        "#55 (0.052)" : {"holeDiameter" : "0.052 in", "tapDrillDiameter" : "0.052 in"},
        "#54 (0.055)" : {"holeDiameter" : "0.055 in", "tapDrillDiameter" : "0.055 in"},
        "#53 (0.0595)" : {"holeDiameter" : "0.0595 in", "tapDrillDiameter" : "0.0595 in"},
        "1/16 (0.0625)" : {"holeDiameter" : "1/16 in", "tapDrillDiameter" : "1/16 in"},
        "#52 (0.0635)" : {"holeDiameter" : "0.0635 in", "tapDrillDiameter" : "0.0635 in"},
        "#51 (0.067)" : {"holeDiameter" : "0.067 in", "tapDrillDiameter" : "0.067 in"},
        "#50 (0.07)" : {"holeDiameter" : "0.07 in", "tapDrillDiameter" : "0.07 in"},
        "#49 (0.073)" : {"holeDiameter" : "0.073 in", "tapDrillDiameter" : "0.073 in"},
        "#48 (0.076)" : {"holeDiameter" : "0.076 in", "tapDrillDiameter" : "0.076 in"},
        "5/64 (0.0781)" : {"holeDiameter" : "5/64 in", "tapDrillDiameter" : "5/64 in"},
        "#47 (0.0785)" : {"holeDiameter" : "0.0785 in", "tapDrillDiameter" : "0.0785 in"},
        "#46 (0.081)" : {"holeDiameter" : "0.081 in", "tapDrillDiameter" : "0.081 in"},
        "#45 (0.082)" : {"holeDiameter" : "0.082 in", "tapDrillDiameter" : "0.082 in"},
        "#44 (0.086)" : {"holeDiameter" : "0.086 in", "tapDrillDiameter" : "0.086 in"},
        "#43 (0.089)" : {"holeDiameter" : "0.089 in", "tapDrillDiameter" : "0.089 in"},
        "#42 (0.0935)" : {"holeDiameter" : "0.0935 in", "tapDrillDiameter" : "0.0935 in"},
        "3/32 (0.0937)" : {"holeDiameter" : "3/32 in", "tapDrillDiameter" : "3/32 in"},
        "#41 (0.096)" : {"holeDiameter" : "0.096 in", "tapDrillDiameter" : "0.096 in"},
        "#40 (0.098)" : {"holeDiameter" : "0.098 in", "tapDrillDiameter" : "0.098 in"},
        "#39 (0.0995)" : {"holeDiameter" : "0.0995 in", "tapDrillDiameter" : "0.0995 in"},
        "#38 (0.1015)" : {"holeDiameter" : "0.1015 in", "tapDrillDiameter" : "0.1015 in"},
        "#37 (0.104)" : {"holeDiameter" : "0.104 in", "tapDrillDiameter" : "0.104 in"},
        "#36 (0.1065)" : {"holeDiameter" : "0.1065 in", "tapDrillDiameter" : "0.1065 in"},
        "7/64 (0.1094)" : {"holeDiameter" : "7/64 in", "tapDrillDiameter" : "7/64 in"},
        "#35 (0.11)" : {"holeDiameter" : "0.11 in", "tapDrillDiameter" : "0.11 in"},
        "#34 (0.111)" : {"holeDiameter" : "0.111 in", "tapDrillDiameter" : "0.111 in"},
        "#33 (0.113)" : {"holeDiameter" : "0.113 in", "tapDrillDiameter" : "0.113 in"},
        "#32 (0.116)" : {"holeDiameter" : "0.116 in", "tapDrillDiameter" : "0.116 in"},
        "#31 (0.12)" : {"holeDiameter" : "0.12 in", "tapDrillDiameter" : "0.12 in"},
        "1/8 (0.125)" : {"holeDiameter" : "1/8 in", "tapDrillDiameter" : "1/8 in"},
        "#30 (0.1285)" : {"holeDiameter" : "0.1285 in", "tapDrillDiameter" : "0.1285 in"},
        "#29 (0.136)" : {"holeDiameter" : "0.136 in", "tapDrillDiameter" : "0.136 in"},
        "#28 (0.1405)" : {"holeDiameter" : "0.1405 in", "tapDrillDiameter" : "0.1405 in"},
        "9/64 (0.1406)" : {"holeDiameter" : "9/64 in", "tapDrillDiameter" : "9/64 in"},
        "#27 (0.144)" : {"holeDiameter" : "0.144 in", "tapDrillDiameter" : "0.144 in"},
        "#26 (0.147)" : {"holeDiameter" : "0.147 in", "tapDrillDiameter" : "0.147 in"},
        "#25 (0.1495)" : {"holeDiameter" : "0.1495 in", "tapDrillDiameter" : "0.1495 in"},
        "#24 (0.152)" : {"holeDiameter" : "0.152 in", "tapDrillDiameter" : "0.152 in"},
        "#23 (0.154)" : {"holeDiameter" : "0.154 in", "tapDrillDiameter" : "0.154 in"},
        "5/32 (0.1562)" : {"holeDiameter" : "5/32 in", "tapDrillDiameter" : "5/32 in"},
        "#22 (0.157)" : {"holeDiameter" : "0.157 in", "tapDrillDiameter" : "0.157 in"},
        "#21 (0.159)" : {"holeDiameter" : "0.159 in", "tapDrillDiameter" : "0.159 in"},
        "#20 (0.161)" : {"holeDiameter" : "0.161 in", "tapDrillDiameter" : "0.161 in"},
        "#19 (0.166)" : {"holeDiameter" : "0.166 in", "tapDrillDiameter" : "0.166 in"},
        "#18 (0.1695)" : {"holeDiameter" : "0.1695 in", "tapDrillDiameter" : "0.1695 in"},
        "11/64 (0.1719)" : {"holeDiameter" : "11/64 in", "tapDrillDiameter" : "11/64 in"},
        "#17 (0.173)" : {"holeDiameter" : "0.173 in", "tapDrillDiameter" : "0.173 in"},
        "#16 (0.177)" : {"holeDiameter" : "0.177 in", "tapDrillDiameter" : "0.177 in"},
        "#15 (0.18)" : {"holeDiameter" : "0.18 in", "tapDrillDiameter" : "0.18 in"},
        "#14 (0.182)" : {"holeDiameter" : "0.182 in", "tapDrillDiameter" : "0.182 in"},
        "#13 (0.185)" : {"holeDiameter" : "0.185 in", "tapDrillDiameter" : "0.185 in"},
        "3/16 (0.1875)" : {"holeDiameter" : "3/16 in", "tapDrillDiameter" : "3/16 in"},
        "#12 (0.189)" : {"holeDiameter" : "0.189 in", "tapDrillDiameter" : "0.189 in"},
        "#11 (0.191)" : {"holeDiameter" : "0.191 in", "tapDrillDiameter" : "0.191 in"},
        "#10 (0.1935)" : {"holeDiameter" : "0.1935 in", "tapDrillDiameter" : "0.1935 in"},
        "#9 (0.196)" : {"holeDiameter" : "0.196 in", "tapDrillDiameter" : "0.196 in"},
        "#8 (0.199)" : {"holeDiameter" : "0.199 in", "tapDrillDiameter" : "0.199 in"},
        "#7 (0.201)" : {"holeDiameter" : "0.201 in", "tapDrillDiameter" : "0.201 in"},
        "13/64 (0.2031)" : {"holeDiameter" : "13/64 in", "tapDrillDiameter" : "13/64 in"},
        "#6 (0.204)" : {"holeDiameter" : "0.204 in", "tapDrillDiameter" : "0.204 in"},
        "#5 (0.2055)" : {"holeDiameter" : "0.2055 in", "tapDrillDiameter" : "0.2055 in"},
        "#4 (0.209)" : {"holeDiameter" : "0.209 in", "tapDrillDiameter" : "0.209 in"},
        "#3 (0.213)" : {"holeDiameter" : "0.213 in", "tapDrillDiameter" : "0.213 in"},
        "7/32 (0.2187)" : {"holeDiameter" : "7/32 in", "tapDrillDiameter" : "7/32 in"},
        "#2 (0.221)" : {"holeDiameter" : "0.221 in", "tapDrillDiameter" : "0.221 in"},
        "#1 (0.228)" : {"holeDiameter" : "0.228 in", "tapDrillDiameter" : "0.228 in"},
        "A (0.234)" : {"holeDiameter" : "0.234 in", "tapDrillDiameter" : "0.234 in"},
        "15/64 (0.2344)" : {"holeDiameter" : "15/64 in", "tapDrillDiameter" : "15/64 in"},
        "B (0.238)" : {"holeDiameter" : "0.238 in", "tapDrillDiameter" : "0.238 in"},
        "C (0.242)" : {"holeDiameter" : "0.242 in", "tapDrillDiameter" : "0.242 in"},
        "D (0.246)" : {"holeDiameter" : "0.246 in", "tapDrillDiameter" : "0.246 in"},
        "1/4 (0.25)" : {"holeDiameter" : "1/4 in", "tapDrillDiameter" : "1/4 in"},
        "E (0.25)" : {"holeDiameter" : "0.25 in", "tapDrillDiameter" : "0.25 in"},
        "F (0.257)" : {"holeDiameter" : "0.257 in", "tapDrillDiameter" : "0.257 in"},
        "G (0.261)" : {"holeDiameter" : "0.261 in", "tapDrillDiameter" : "0.261 in"},
        "17/64 (0.2656)" : {"holeDiameter" : "17/64 in", "tapDrillDiameter" : "17/64 in"},
        "H (0.266)" : {"holeDiameter" : "0.266 in", "tapDrillDiameter" : "0.266 in"},
        "I (0.272)" : {"holeDiameter" : "0.272 in", "tapDrillDiameter" : "0.272 in"},
        "J (0.277)" : {"holeDiameter" : "0.277 in", "tapDrillDiameter" : "0.277 in"},
        "K (0.2811)" : {"holeDiameter" : "0.2811 in", "tapDrillDiameter" : "0.2811 in"},
        "9/32 (0.2812)" : {"holeDiameter" : "9/32 in", "tapDrillDiameter" : "9/32 in"},
        "L (0.29)" : {"holeDiameter" : "0.29 in", "tapDrillDiameter" : "0.29 in"},
        "M (0.295)" : {"holeDiameter" : "0.295 in", "tapDrillDiameter" : "0.295 in"},
        "19/64 (0.2968)" : {"holeDiameter" : "19/64 in", "tapDrillDiameter" : "19/64 in"},
        "N (0.302)" : {"holeDiameter" : "0.302 in", "tapDrillDiameter" : "0.302 in"},
        "5/16 (0.3125)" : {"holeDiameter" : "5/16 in", "tapDrillDiameter" : "5/16 in"},
        "O (0.316)" : {"holeDiameter" : "0.316 in", "tapDrillDiameter" : "0.316 in"},
        "P (0.323)" : {"holeDiameter" : "0.323 in", "tapDrillDiameter" : "0.323 in"},
        "21/64 (0.3281)" : {"holeDiameter" : "21/64 in", "tapDrillDiameter" : "21/64 in"},
        "Q (0.332)" : {"holeDiameter" : "0.332 in", "tapDrillDiameter" : "0.332 in"},
        "R (0.339)" : {"holeDiameter" : "0.339 in", "tapDrillDiameter" : "0.339 in"},
        "11/32 (0.3437)" : {"holeDiameter" : "11/32 in", "tapDrillDiameter" : "11/32 in"},
        "S (0.348)" : {"holeDiameter" : "0.348 in", "tapDrillDiameter" : "0.348 in"},
        "T (0.358)" : {"holeDiameter" : "0.358 in", "tapDrillDiameter" : "0.358 in"},
        "23/64 (0.3594)" : {"holeDiameter" : "23/64 in", "tapDrillDiameter" : "23/64 in"},
        "U (0.368)" : {"holeDiameter" : "0.368 in", "tapDrillDiameter" : "0.368 in"},
        "3/8 (0.375)" : {"holeDiameter" : "3/8 in", "tapDrillDiameter" : "3/8 in"},
        "V (0.377)" : {"holeDiameter" : "0.377 in", "tapDrillDiameter" : "0.377 in"},
        "W (0.386)" : {"holeDiameter" : "0.386 in", "tapDrillDiameter" : "0.386 in"},
        "25/64 (0.3906)" : {"holeDiameter" : "25/64 in", "tapDrillDiameter" : "25/64 in"},
        "X (0.397)" : {"holeDiameter" : "0.397 in", "tapDrillDiameter" : "0.397 in"},
        "Y (0.404)" : {"holeDiameter" : "0.404 in", "tapDrillDiameter" : "0.404 in"},
        "13/32 (0.4062)" : {"holeDiameter" : "13/32 in", "tapDrillDiameter" : "13/32 in"},
        "Z (0.413)" : {"holeDiameter" : "0.413 in", "tapDrillDiameter" : "0.413 in"},
        "27/64 (0.4219)" : {"holeDiameter" : "27/64 in", "tapDrillDiameter" : "27/64 in"},
        "7/16 (0.4375)" : {"holeDiameter" : "7/16 in", "tapDrillDiameter" : "7/16 in"},
        "29/64 (0.4531)" : {"holeDiameter" : "29/64 in", "tapDrillDiameter" : "29/64 in"},
        "15/32 (0.4687)" : {"holeDiameter" : "15/32 in", "tapDrillDiameter" : "15/32 in"},
        "31/64 (0.4844)" : {"holeDiameter" : "31/64 in", "tapDrillDiameter" : "31/64 in"},
        "1/2 (0.5)" : {"holeDiameter" : "1/2 in", "tapDrillDiameter" : "1/2 in"},
        "33/64 (0.5156)" : {"holeDiameter" : "33/64 in", "tapDrillDiameter" : "33/64 in"},
        "17/32 (0.5312)" : {"holeDiameter" : "17/32 in", "tapDrillDiameter" : "17/32 in"},
        "35/64 (0.5469)" : {"holeDiameter" : "35/64 in", "tapDrillDiameter" : "35/64 in"},
        "9/16 (0.5625)" : {"holeDiameter" : "9/16 in", "tapDrillDiameter" : "9/16 in"},
        "37/64 (0.5781)" : {"holeDiameter" : "37/64 in", "tapDrillDiameter" : "37/64 in"},
        "19/32 (0.5937)" : {"holeDiameter" : "19/32 in", "tapDrillDiameter" : "19/32 in"},
        "39/64 (0.6094)" : {"holeDiameter" : "39/64 in", "tapDrillDiameter" : "39/64 in"},
        "5/8 (0.625)" : {"holeDiameter" : "5/8 in", "tapDrillDiameter" : "5/8 in"},
        "41/64 (0.6406)" : {"holeDiameter" : "41/64 in", "tapDrillDiameter" : "41/64 in"},
        "21/32 (0.6562)" : {"holeDiameter" : "21/32 in", "tapDrillDiameter" : "21/32 in"},
        "43/64 (0.6719)" : {"holeDiameter" : "43/64 in", "tapDrillDiameter" : "43/64 in"},
        "11/16 (0.6875)" : {"holeDiameter" : "11/16 in", "tapDrillDiameter" : "11/16 in"},
        "45/64 (0.7031)" : {"holeDiameter" : "45/64 in", "tapDrillDiameter" : "45/64 in"},
        "23/32 (0.7187)" : {"holeDiameter" : "23/32 in", "tapDrillDiameter" : "23/32 in"},
        "47/64 (0.7344)" : {"holeDiameter" : "47/64 in", "tapDrillDiameter" : "47/64 in"},
        "3/4 (0.75)" : {"holeDiameter" : "3/4 in", "tapDrillDiameter" : "3/4 in"},
        "49/64 (0.7656)" : {"holeDiameter" : "49/64 in", "tapDrillDiameter" : "49/64 in"},
        "25/32 (0.7812)" : {"holeDiameter" : "25/32 in", "tapDrillDiameter" : "25/32 in"},
        "51/64 (0.7969)" : {"holeDiameter" : "51/64 in", "tapDrillDiameter" : "51/64 in"},
        "13/16 (0.8125)" : {"holeDiameter" : "13/16 in", "tapDrillDiameter" : "13/16 in"},
        "53/64 (0.8281)" : {"holeDiameter" : "53/64 in", "tapDrillDiameter" : "53/64 in"},
        "27/32 (0.8437)" : {"holeDiameter" : "27/32 in", "tapDrillDiameter" : "27/32 in"},
        "55/64 (0.8594)" : {"holeDiameter" : "55/64 in", "tapDrillDiameter" : "55/64 in"},
        "7/8 (0.875)" : {"holeDiameter" : "7/8 in", "tapDrillDiameter" : "7/8 in"},
        "57/64 (0.8906)" : {"holeDiameter" : "57/64 in", "tapDrillDiameter" : "57/64 in"},
        "29/32 (0.9062)" : {"holeDiameter" : "29/32 in", "tapDrillDiameter" : "29/32 in"},
        "59/64 (0.9219)" : {"holeDiameter" : "59/64 in", "tapDrillDiameter" : "59/64 in"},
        "15/16 (0.9375)" : {"holeDiameter" : "15/16 in", "tapDrillDiameter" : "15/16 in"},
        "61/64 (0.9531)" : {"holeDiameter" : "61/64 in", "tapDrillDiameter" : "61/64 in"},
        "31/32 (0.9687)" : {"holeDiameter" : "31/32 in", "tapDrillDiameter" : "31/32 in"},
        "63/64 (0.9844)" : {"holeDiameter" : "63/64 in", "tapDrillDiameter" : "63/64 in"},
        "1 (1.0)" : {"holeDiameter" : "1 in", "tapDrillDiameter" : "1 in"}
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
                "Close" : {"holeDiameter" : "0.0635 in", "cBoreDiameter" : "1/8 in", "cBoreDepth" : "0.06 in", "cSinkDiameter" : "0.138 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "0.07 in", "cBoreDiameter" : "1/8 in", "cBoreDepth" : "0.06 in", "cSinkDiameter" : "0.138 in", "cSinkAngle" : "82 degree"},
                "Close (ASME)" : {"holeDiameter" : "0.067 in", "cBoreDiameter" : "1/8 in", "cBoreDepth" : "0.06 in", "cSinkDiameter" : "0.138 in", "cSinkAngle" : "82 degree"},
                "Normal (ASME)" : {"holeDiameter" : "0.076 in", "cBoreDiameter" : "1/8 in", "cBoreDepth" : "0.06 in", "cSinkDiameter" : "0.138 in", "cSinkAngle" : "82 degree"},
                "Loose (ASME)" : {"holeDiameter" : "0.094 in", "cBoreDiameter" : "1/8 in", "cBoreDepth" : "0.06 in", "cSinkDiameter" : "0.138 in", "cSinkAngle" : "82 degree"}
            }
        },
        "#1" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.076 in", "cBoreDiameter" : "5/32 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "0.081 in", "cBoreDiameter" : "5/32 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree"},
                "Close (ASME)" : {"holeDiameter" : "0.081 in", "cBoreDiameter" : "5/32 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree"},
                "Normal (ASME)" : {"holeDiameter" : "0.089 in", "cBoreDiameter" : "5/32 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree"},
                "Loose (ASME)" : {"holeDiameter" : "0.104 in", "cBoreDiameter" : "5/32 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree"}
            }
        },
        "#2" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.089 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "0.096 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree"},
                "Close (ASME)" : {"holeDiameter" : "0.094 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree"},
                "Normal (ASME)" : {"holeDiameter" : "0.102 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree"},
                "Loose (ASME)" : {"holeDiameter" : "0.116 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree"}
            }
        },
        "#3" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.104 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "0.11 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree"},
                "Close (ASME)" : {"holeDiameter" : "0.106 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree"},
                "Normal (ASME)" : {"holeDiameter" : "0.116 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree"},
                "Loose (ASME)" : {"holeDiameter" : "0.128 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree"}
            }
        },
        "#4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.116 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "0.1285 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree"},
                "Close (ASME)" : {"holeDiameter" : "0.12 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree"},
                "Normal (ASME)" : {"holeDiameter" : "0.128 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree"},
                "Loose (ASME)" : {"holeDiameter" : "0.144 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree"}
            }
        },
        "#5" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.1285 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "0.136 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree"},
                "Close (ASME)" : {"holeDiameter" : "0.141 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree"},
                "Normal (ASME)" : {"holeDiameter" : "0.156 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree"},
                "Loose (ASME)" : {"holeDiameter" : "0.172 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree"}
            }
        },
        "#6" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.144 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "0.1495 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree"},
                "Close (ASME)" : {"holeDiameter" : "0.154 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree"},
                "Normal (ASME)" : {"holeDiameter" : "0.17 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree"},
                "Loose (ASME)" : {"holeDiameter" : "0.185 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree"}
            }
        },
        "#8" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.1695 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "0.177 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree"},
                "Close (ASME)" : {"holeDiameter" : "0.18 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree"},
                "Normal (ASME)" : {"holeDiameter" : "0.196 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree"},
                "Loose (ASME)" : {"holeDiameter" : "0.213 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree"}
            }
        },
        "#10" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.196 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "0.201 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree"},
                "Close (ASME)" : {"holeDiameter" : "0.206 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree"},
                "Normal (ASME)" : {"holeDiameter" : "0.221 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree"},
                "Loose (ASME)" : {"holeDiameter" : "0.238 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree"}
            }
        },
        "#12" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.221 in", "cBoreDiameter" : "13/32 in", "cBoreDepth" : "0.216 in", "cSinkDiameter" : "0.45 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "0.228 in", "cBoreDiameter" : "13/32 in", "cBoreDepth" : "0.216 in", "cSinkDiameter" : "0.45 in", "cSinkAngle" : "82 degree"}
            }
        },
        "1/4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.257 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "0.266 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree"},
                "Close (ASME)" : {"holeDiameter" : "0.266 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree"},
                "Normal (ASME)" : {"holeDiameter" : "0.281 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree"},
                "Loose (ASME)" : {"holeDiameter" : "0.297 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree"}
            }
        },
        "5/16" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.323 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "0.332 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree"},
                "Close (ASME)" : {"holeDiameter" : "0.328 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree"},
                "Normal (ASME)" : {"holeDiameter" : "0.344 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree"},
                "Loose (ASME)" : {"holeDiameter" : "0.359 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree"}
            }
        },
        "3/8" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.386 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "0.397 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree"},
                "Close (ASME)" : {"holeDiameter" : "0.391 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree"},
                "Normal (ASME)" : {"holeDiameter" : "0.406 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree"},
                "Loose (ASME)" : {"holeDiameter" : "0.422 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree"}
            }
        },
        "7/16" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.4531 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "0.4687 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree"},
                "Close (ASME)" : {"holeDiameter" : "0.453 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree"},
                "Normal (ASME)" : {"holeDiameter" : "0.469 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree"},
                "Loose (ASME)" : {"holeDiameter" : "0.484 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree"}
            }
        },
        "1/2" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.5156 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "0.5312 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree"},
                "Close (ASME)" : {"holeDiameter" : "0.531 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree"},
                "Normal (ASME)" : {"holeDiameter" : "0.562 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree"},
                "Loose (ASME)" : {"holeDiameter" : "0.609 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree"}
            }
        },
        "9/16" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.5781 in", "cBoreDiameter" : "29/32 in", "cBoreDepth" : "0.5625 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "0.5938 in", "cBoreDiameter" : "29/32 in", "cBoreDepth" : "0.5625 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree"}
            }
        },
        "5/8" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.6406 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "0.6562 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree"},
                "Close (ASME)" : {"holeDiameter" : "0.656 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree"},
                "Normal (ASME)" : {"holeDiameter" : "0.688 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree"},
                "Loose (ASME)" : {"holeDiameter" : "0.734 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree"}
            }
        },
        "3/4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.7656 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "0.7812 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree"},
                "Close (ASME)" : {"holeDiameter" : "0.781 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree"},
                "Normal (ASME)" : {"holeDiameter" : "0.812 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree"},
                "Loose (ASME)" : {"holeDiameter" : "0.906 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree"}
            }
        },
        "7/8" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "0.8906 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "0.9062 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree"},
                "Close (ASME)" : {"holeDiameter" : "0.906 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree"},
                "Normal (ASME)" : {"holeDiameter" : "0.938 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree"},
                "Loose (ASME)" : {"holeDiameter" : "1.031 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree"}
            }
        },
        "1" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "1.0156 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "1.0313 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree"},
                "Close (ASME)" : {"holeDiameter" : "1.031 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree"},
                "Normal (ASME)" : {"holeDiameter" : "1.094 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree"},
                "Loose (ASME)" : {"holeDiameter" : "1.156 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree"}
            }
        },
        "1 1/8" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "1.1406 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "1.1562 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree"},
                "Close (ASME)" : {"holeDiameter" : "1.156 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree"},
                "Normal (ASME)" : {"holeDiameter" : "1.219 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree"},
                "Loose (ASME)" : {"holeDiameter" : "1.312 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree"}
            }
        },
        "1 1/4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "1.2656 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "1.2812 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree"},
                "Close (ASME)" : {"holeDiameter" : "1.281 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree"},
                "Normal (ASME)" : {"holeDiameter" : "1.344 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree"},
                "Loose (ASME)" : {"holeDiameter" : "1.438 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree"}
            }
        },
        "1 3/8" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "1.3906 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "1.4062 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree"},
                "Close (ASME)" : {"holeDiameter" : "1.438 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree"},
                "Normal (ASME)" : {"holeDiameter" : "1.5 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree"},
                "Loose (ASME)" : {"holeDiameter" : "1.609 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree"}
            }
        },
        "1 1/2" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "1.5156 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "1.5312 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree"},
                "Close (ASME)" : {"holeDiameter" : "1.562 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree"},
                "Normal (ASME)" : {"holeDiameter" : "1.625 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree"},
                "Loose (ASME)" : {"holeDiameter" : "1.734 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree"}
            }
        },
        "1 3/4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "1.7656 in", "cBoreDiameter" : "2 3/4 in", "cBoreDepth" : "1.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "1.7812 in", "cBoreDiameter" : "2 3/4 in", "cBoreDepth" : "1.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree"}
            }
        },
        "2" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "2.0156 in", "cBoreDiameter" : "3.125 in", "cBoreDepth" : "2 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "2.0312 in", "cBoreDiameter" : "3.125 in", "cBoreDepth" : "2 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree"}
            }
        },
        "2 1/4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "2.2656 in", "cBoreDiameter" : "3.5 in", "cBoreDepth" : "2.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "2.2812 in", "cBoreDiameter" : "3.5 in", "cBoreDepth" : "2.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree"}
            }
        },
        "2 1/2" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "2.5156 in", "cBoreDiameter" : "3.875 in", "cBoreDepth" : "2.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "2.5312 in", "cBoreDiameter" : "3.875 in", "cBoreDepth" : "2.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree"}
            }
        },
        "2 3/4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "2.7656 in", "cBoreDiameter" : "4.25 in", "cBoreDepth" : "2.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "2.7812 in", "cBoreDiameter" : "4.25 in", "cBoreDepth" : "2.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree"}
            }
        },
        "3" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "3.0156 in", "cBoreDiameter" : "4.625 in", "cBoreDepth" : "3 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "3.0312 in", "cBoreDiameter" : "4.625 in", "cBoreDepth" : "3 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree"}
            }
        },
        "3 1/4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "3.2656 in", "cBoreDiameter" : "5 in", "cBoreDepth" : "3.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "3.2812 in", "cBoreDiameter" : "5 in", "cBoreDepth" : "3.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree"}
            }
        },
        "3 1/2" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "3.5156 in", "cBoreDiameter" : "5.375 in", "cBoreDepth" : "3.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "3.5312 in", "cBoreDiameter" : "5.375 in", "cBoreDepth" : "3.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree"}
            }
        },
        "3 3/4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "3.7656 in", "cBoreDiameter" : "5.75 in", "cBoreDepth" : "3.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "3.7812 in", "cBoreDiameter" : "5.75 in", "cBoreDepth" : "3.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree"}
            }
        },
        "4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Free",
            "entries" : {
                "Close" : {"holeDiameter" : "4.0156 in", "cBoreDiameter" : "6.125 in", "cBoreDepth" : "4 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree"},
                "Free" : {"holeDiameter" : "4.0312 in", "cBoreDiameter" : "6.125 in", "cBoreDepth" : "4 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree"}
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
                        "50%" : {"holeDiameter" : "0.052 in", "tapDrillDiameter" : "0.052 in", "cBoreDiameter" : "1/8 in", "cBoreDepth" : "0.06 in", "cSinkDiameter" : "0.138 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.06 in"},
                        "75%" : {"holeDiameter" : "0.0469 in", "tapDrillDiameter" : "0.0469 in", "cBoreDiameter" : "1/8 in", "cBoreDepth" : "0.06 in", "cSinkDiameter" : "0.138 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.06 in"}
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
                        "50%" : {"holeDiameter" : "0.0625 in", "tapDrillDiameter" : "0.0625 in", "cBoreDiameter" : "5/32 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.073 in"},
                        "75%" : {"holeDiameter" : "0.0595 in", "tapDrillDiameter" : "0.0595 in", "cBoreDiameter" : "5/32 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.073 in"}
                    }
                },
                "72 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.0635 in", "tapDrillDiameter" : "0.0635 in", "cBoreDiameter" : "5/33 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.073 in"},
                        "75%" : {"holeDiameter" : "0.0595 in", "tapDrillDiameter" : "0.0595 in", "cBoreDiameter" : "5/33 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.073 in"}
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
                        "50%" : {"holeDiameter" : "0.073 in", "tapDrillDiameter" : "0.073 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.086 in"},
                        "75%" : {"holeDiameter" : "0.07 in", "tapDrillDiameter" : "0.07 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.086 in"}
                    }
                },
                "64 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.076 in", "tapDrillDiameter" : "0.076 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.086 in"},
                        "75%" : {"holeDiameter" : "0.07 in", "tapDrillDiameter" : "0.07 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.086 in"}
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
                        "50%" : {"holeDiameter" : "0.086 in", "tapDrillDiameter" : "0.086 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.099 in"},
                        "75%" : {"holeDiameter" : "0.0785 in", "tapDrillDiameter" : "0.0785 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.099 in"}
                    }
                },
                "56 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.089 in", "tapDrillDiameter" : "0.089 in", "cBoreDiameter" : "7/33 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.099 in"},
                        "75%" : {"holeDiameter" : "0.082 in", "tapDrillDiameter" : "0.082 in", "cBoreDiameter" : "7/33 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.099 in"}
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
                        "50%" : {"holeDiameter" : "0.096 in", "tapDrillDiameter" : "0.096 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.112 in"},
                        "75%" : {"holeDiameter" : "0.089 in", "tapDrillDiameter" : "0.089 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.112 in"}
                    }
                },
                "48 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.098 in", "tapDrillDiameter" : "0.098 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.112 in"},
                        "75%" : {"holeDiameter" : "0.0935 in", "tapDrillDiameter" : "0.0935 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.112 in"}
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
                        "50%" : {"holeDiameter" : "0.1094 in", "tapDrillDiameter" : "0.1094 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.125 in"},
                        "75%" : {"holeDiameter" : "0.1015 in", "tapDrillDiameter" : "0.1015 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.125 in"}
                    }
                },
                "44 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.11 in", "tapDrillDiameter" : "0.11 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.125 in"},
                        "75%" : {"holeDiameter" : "0.104 in", "tapDrillDiameter" : "0.104 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.125 in"}
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
                        "50%" : {"holeDiameter" : "0.116 in", "tapDrillDiameter" : "0.116 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.138 in"},
                        "75%" : {"holeDiameter" : "0.1065 in", "tapDrillDiameter" : "0.1065 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.138 in"}
                    }
                },
                "40 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.12 in", "tapDrillDiameter" : "0.12 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.138 in"},
                        "75%" : {"holeDiameter" : "0.113 in", "tapDrillDiameter" : "0.113 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.138 in"}
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
                        "50%" : {"holeDiameter" : "0.144 in", "tapDrillDiameter" : "0.144 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.164 in"},
                        "75%" : {"holeDiameter" : "0.136 in", "tapDrillDiameter" : "0.136 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.164 in"}
                    }
                },
                "36 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.147 in", "tapDrillDiameter" : "0.147 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.164 in"},
                        "75%" : {"holeDiameter" : "0.136 in", "tapDrillDiameter" : "0.136 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.164 in"}
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
                        "50%" : {"holeDiameter" : "0.161 in", "tapDrillDiameter" : "0.161 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.19 in"},
                        "75%" : {"holeDiameter" : "0.1495 in", "tapDrillDiameter" : "0.1495 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.19 in"}
                    }
                },
                "32 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.1695 in", "tapDrillDiameter" : "0.1695 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.19 in"},
                        "75%" : {"holeDiameter" : "0.159 in", "tapDrillDiameter" : "0.159 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.19 in"}
                    }
                }
            }
        },
        "#12" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "24 tpi",
            "entries" : {
                "24 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.189 in", "tapDrillDiameter" : "0.189 in", "cBoreDiameter" : "13/32 in", "cBoreDepth" : "0.216 in", "cSinkDiameter" : "0.45 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.216 in"},
                        "75%" : {"holeDiameter" : "0.177 in", "tapDrillDiameter" : "0.177 in", "cBoreDiameter" : "13/32 in", "cBoreDepth" : "0.216 in", "cSinkDiameter" : "0.45 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.216 in"}
                    }
                },
                "28 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.1935 in", "tapDrillDiameter" : "0.1935 in", "cBoreDiameter" : "13/32 in", "cBoreDepth" : "0.216 in", "cSinkDiameter" : "0.45 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.216 in"},
                        "75%" : {"holeDiameter" : "0.182 in", "tapDrillDiameter" : "0.182 in", "cBoreDiameter" : "13/32 in", "cBoreDepth" : "0.216 in", "cSinkDiameter" : "0.45 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.216 in"}
                    }
                },
                "32 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.196 in", "tapDrillDiameter" : "0.196 in", "cBoreDiameter" : "13/32 in", "cBoreDepth" : "0.216 in", "cSinkDiameter" : "0.45 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.216 in"},
                        "75%" : {"holeDiameter" : "0.185 in", "tapDrillDiameter" : "0.185 in", "cBoreDiameter" : "13/32 in", "cBoreDepth" : "0.216 in", "cSinkDiameter" : "0.45 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.216 in"}
                    }
                }
            }
        },
        "1/4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "20 tpi",
            "entries" : {
                "20 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.2188 in", "tapDrillDiameter" : "0.2188 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"},
                        "75%" : {"holeDiameter" : "0.201 in", "tapDrillDiameter" : "0.201 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"}
                    }
                },
                "28 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.228 in", "tapDrillDiameter" : "0.228 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"},
                        "75%" : {"holeDiameter" : "0.213 in", "tapDrillDiameter" : "0.213 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"}
                    }
                },
                "32 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.228 in", "tapDrillDiameter" : "0.228 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"},
                        "75%" : {"holeDiameter" : "0.2188 in", "tapDrillDiameter" : "0.2188 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"}
                    }
                }
            }
        },
        "5/16" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "18 tpi",
            "entries" : {
                "18 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.277 in", "tapDrillDiameter" : "0.277 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"},
                        "75%" : {"holeDiameter" : "0.257 in", "tapDrillDiameter" : "0.257 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"}
                    }
                },
                "24 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.2812 in", "tapDrillDiameter" : "0.2812 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"},
                        "75%" : {"holeDiameter" : "0.272 in", "tapDrillDiameter" : "0.272 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"}
                    }
                },
                "32 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.29 in", "tapDrillDiameter" : "0.29 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"},
                        "75%" : {"holeDiameter" : "0.2812 in", "tapDrillDiameter" : "0.2812 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"}
                    }
                }
            }
        },
        "3/8" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "16 tpi",
            "entries" : {
                "16 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.332 in", "tapDrillDiameter" : "0.332 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"},
                        "75%" : {"holeDiameter" : "0.3125 in", "tapDrillDiameter" : "0.3125 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"}
                    }
                },
                "24 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.348 in", "tapDrillDiameter" : "0.348 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"},
                        "75%" : {"holeDiameter" : "0.332 in", "tapDrillDiameter" : "0.332 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"}
                    }
                },
                "32 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.358 in", "tapDrillDiameter" : "0.358 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"},
                        "75%" : {"holeDiameter" : "0.3438 in", "tapDrillDiameter" : "0.3438 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"}
                    }
                }
            }
        },
        "7/16" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "14 tpi",
            "entries" : {
                "14 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.3906 in", "tapDrillDiameter" : "0.3906 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"},
                        "75%" : {"holeDiameter" : "0.368 in", "tapDrillDiameter" : "0.368 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"}
                    }
                },
                "20 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.4062 in", "tapDrillDiameter" : "0.4062 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"},
                        "75%" : {"holeDiameter" : "0.3906 in", "tapDrillDiameter" : "0.3906 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"}
                    }
                },
                "28 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.413 in", "tapDrillDiameter" : "0.413 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"},
                        "75%" : {"holeDiameter" : "0.404 in", "tapDrillDiameter" : "0.404 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"}
                    }
                }
            }
        },
        "1/2" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "13 tpi",
            "entries" : {
                "13 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.4531 in", "tapDrillDiameter" : "0.4531 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"},
                        "75%" : {"holeDiameter" : "0.4219 in", "tapDrillDiameter" : "0.4219 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"}
                    }
                },
                "20 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.4688 in", "tapDrillDiameter" : "0.4688 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"},
                        "75%" : {"holeDiameter" : "0.4531 in", "tapDrillDiameter" : "0.4531 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"}
                    }
                },
                "28 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.4688 in", "tapDrillDiameter" : "0.4688 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"},
                        "75%" : {"holeDiameter" : "0.4688 in", "tapDrillDiameter" : "0.4688 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"}
                    }
                }
            }
        },
        "9/16" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "12 tpi",
            "entries" : {
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.5156 in", "tapDrillDiameter" : "0.5156 in", "cBoreDiameter" : "29/32 in", "cBoreDepth" : "0.5625 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5625 in"},
                        "75%" : {"holeDiameter" : "0.4844 in", "tapDrillDiameter" : "0.4844 in", "cBoreDiameter" : "29/32 in", "cBoreDepth" : "0.5625 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5625 in"}
                    }
                },
                "18 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.5312 in", "tapDrillDiameter" : "0.5312 in", "cBoreDiameter" : "29/32 in", "cBoreDepth" : "0.5625 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5625 in"},
                        "75%" : {"holeDiameter" : "0.5156 in", "tapDrillDiameter" : "0.5156 in", "cBoreDiameter" : "29/32 in", "cBoreDepth" : "0.5625 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5625 in"}
                    }
                },
                "24 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.5312 in", "tapDrillDiameter" : "0.5312 in", "cBoreDiameter" : "29/32 in", "cBoreDepth" : "0.5625 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5625 in"},
                        "75%" : {"holeDiameter" : "0.5156 in", "tapDrillDiameter" : "0.5156 in", "cBoreDiameter" : "29/32 in", "cBoreDepth" : "0.5625 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5625 in"}
                    }
                }
            }
        },
        "5/8" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "11 tpi",
            "entries" : {
                "11 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.5625 in", "tapDrillDiameter" : "0.5625 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"},
                        "75%" : {"holeDiameter" : "0.5312 in", "tapDrillDiameter" : "0.5312 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"}
                    }
                },
                "18 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.5938 in", "tapDrillDiameter" : "0.5938 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"},
                        "75%" : {"holeDiameter" : "0.5781 in", "tapDrillDiameter" : "0.5781 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"}
                    }
                },
                "24 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.5938 in", "tapDrillDiameter" : "0.5938 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"},
                        "75%" : {"holeDiameter" : "0.5781 in", "tapDrillDiameter" : "0.5781 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"}
                    }
                }
            }
        },
        "3/4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "10 tpi",
            "entries" : {
                "10 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.6875 in", "tapDrillDiameter" : "0.6875 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"},
                        "75%" : {"holeDiameter" : "0.6562 in", "tapDrillDiameter" : "0.6562 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"}
                    }
                },
                "16 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.7031 in", "tapDrillDiameter" : "0.7031 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"},
                        "75%" : {"holeDiameter" : "0.6875 in", "tapDrillDiameter" : "0.6875 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"}
                    }
                },
                "20 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.7188 in", "tapDrillDiameter" : "0.7188 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"},
                        "75%" : {"holeDiameter" : "0.7031 in", "tapDrillDiameter" : "0.7031 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"}
                    }
                }
            }
        },
        "7/8" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "9 tpi",
            "entries" : {
                "9 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.7969 in", "tapDrillDiameter" : "0.7969 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"},
                        "75%" : {"holeDiameter" : "0.7656 in", "tapDrillDiameter" : "0.7656 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"}
                    }
                },
                "14 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.8281 in", "tapDrillDiameter" : "0.8281 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"},
                        "75%" : {"holeDiameter" : "0.8125 in", "tapDrillDiameter" : "0.8125 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"}
                    }
                },
                "20 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.8438 in", "tapDrillDiameter" : "0.8438 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"},
                        "75%" : {"holeDiameter" : "0.8281 in", "tapDrillDiameter" : "0.8281 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"}
                    }
                }
            }
        },
        "1" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "8 tpi",
            "entries" : {
                "8 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.9219 in", "tapDrillDiameter" : "0.9219 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"},
                        "75%" : {"holeDiameter" : "0.875 in", "tapDrillDiameter" : "0.875 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.9531 in", "tapDrillDiameter" : "0.9531 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"},
                        "75%" : {"holeDiameter" : "0.9219 in", "tapDrillDiameter" : "0.9219 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"}
                    }
                },
                "20 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.9688 in", "tapDrillDiameter" : "0.9688 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"},
                        "75%" : {"holeDiameter" : "0.9531 in", "tapDrillDiameter" : "0.9531 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"}
                    }
                }
            }
        },
        "1 1/8" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "7 tpi",
            "entries" : {
                "7 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.0313 in", "tapDrillDiameter" : "1.0313 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.125 in"},
                        "75%" : {"holeDiameter" : "0.9844 in", "tapDrillDiameter" : "0.9844 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.125 in"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.0781 in", "tapDrillDiameter" : "1.0781 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.125 in"},
                        "75%" : {"holeDiameter" : "1.0469 in", "tapDrillDiameter" : "1.0469 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.125 in"}
                    }
                }
            }
        },
        "1 1/4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "7 tpi",
            "entries" : {
                "7 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.1562 in", "tapDrillDiameter" : "1.1562 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"},
                        "75%" : {"holeDiameter" : "1.1094 in", "tapDrillDiameter" : "1.1094 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.2031 in", "tapDrillDiameter" : "1.2031 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"},
                        "75%" : {"holeDiameter" : "1.1719 in", "tapDrillDiameter" : "1.1719 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"}
                    }
                },
                "18 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.2187 in", "tapDrillDiameter" : "1.2187 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"},
                        "75%" : {"holeDiameter" : "1.1875 in", "tapDrillDiameter" : "1.1875 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"}
                    }
                }
            }
        },
        "1 3/8" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "6 tpi",
            "entries" : {
                "6 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.2656 in", "tapDrillDiameter" : "1.2656 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.375 in"},
                        "75%" : {"holeDiameter" : "1.2187 in", "tapDrillDiameter" : "1.2187 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.375 in"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.3281 in", "tapDrillDiameter" : "1.3281 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.375 in"},
                        "75%" : {"holeDiameter" : "1.2969 in", "tapDrillDiameter" : "1.2969 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.375 in"}
                    }
                }
            }
        },
        "1 1/2" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "6 tpi",
            "entries" : {
                "6 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.3906 in", "tapDrillDiameter" : "1.3906 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"},
                        "75%" : {"holeDiameter" : "1.3437 in", "tapDrillDiameter" : "1.3437 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.4375 in", "tapDrillDiameter" : "1.4375 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"},
                        "75%" : {"holeDiameter" : "1.4219 in", "tapDrillDiameter" : "1.4219 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"}
                    }
                },
                "18 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.4687 in", "tapDrillDiameter" : "1.4687 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"},
                        "75%" : {"holeDiameter" : "1.4375 in", "tapDrillDiameter" : "1.4375 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"}
                    }
                }
            }
        },
        "1 3/4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "5 tpi",
            "entries" : {
                "5 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.6417 in", "tapDrillDiameter" : "1.6417 in", "cBoreDiameter" : "2 3/4 in", "cBoreDepth" : "1.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.75 in"},
                        "75%" : {"holeDiameter" : "1.5625 in", "tapDrillDiameter" : "1.5625 in", "cBoreDiameter" : "2 3/4 in", "cBoreDepth" : "1.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.75 in"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.6959 in", "tapDrillDiameter" : "1.6959 in", "cBoreDiameter" : "2 3/4 in", "cBoreDepth" : "1.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.75 in"},
                        "75%" : {"holeDiameter" : "1.6735 in", "tapDrillDiameter" : "1.6735 in", "cBoreDiameter" : "2 3/4 in", "cBoreDepth" : "1.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.75 in"}
                    }
                },
                "16 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.7094 in", "tapDrillDiameter" : "1.7094 in", "cBoreDiameter" : "2 3/4 in", "cBoreDepth" : "1.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.75 in"},
                        "75%" : {"holeDiameter" : "1.6925 in", "tapDrillDiameter" : "1.6925 in", "cBoreDiameter" : "2 3/4 in", "cBoreDepth" : "1.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.75 in"}
                    }
                }
            }
        },
        "2" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "4 1/2 tpi",
            "entries" : {
                "4 1/2 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.8594 in", "tapDrillDiameter" : "1.8594 in", "cBoreDiameter" : "3.125 in", "cBoreDepth" : "2 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2 in"},
                        "75%" : {"holeDiameter" : "1.7812 in", "tapDrillDiameter" : "1.7812 in", "cBoreDiameter" : "3.125 in", "cBoreDepth" : "2 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2 in"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.9531 in", "tapDrillDiameter" : "1.9531 in", "cBoreDiameter" : "3.125 in", "cBoreDepth" : "2 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2 in"},
                        "75%" : {"holeDiameter" : "1.9219 in", "tapDrillDiameter" : "1.9219 in", "cBoreDiameter" : "3.125 in", "cBoreDepth" : "2 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2 in"}
                    }
                }
            }
        },
        "2 1/4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "4 1/2 tpi",
            "entries" : {
                "4 1/2 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "2.1057 in", "tapDrillDiameter" : "2.1057 in", "cBoreDiameter" : "3.5 in", "cBoreDepth" : "2.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.25 in"},
                        "75%" : {"holeDiameter" : "2.036 in", "tapDrillDiameter" : "2.036 in", "cBoreDiameter" : "3.5 in", "cBoreDepth" : "2.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.25 in"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "2.1959 in", "tapDrillDiameter" : "2.1959 in", "cBoreDiameter" : "3.5 in", "cBoreDepth" : "2.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.25 in"},
                        "75%" : {"holeDiameter" : "2.1735 in", "tapDrillDiameter" : "2.1735 in", "cBoreDiameter" : "3.5 in", "cBoreDepth" : "2.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.25 in"}
                    }
                }
            }
        },
        "2 1/2" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "4 tpi",
            "entries" : {
                "4 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "2.3376 in", "tapDrillDiameter" : "2.3376 in", "cBoreDiameter" : "3.875 in", "cBoreDepth" : "2.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.5 in"},
                        "75%" : {"holeDiameter" : "2.2575 in", "tapDrillDiameter" : "2.2575 in", "cBoreDiameter" : "3.875 in", "cBoreDepth" : "2.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.5 in"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "2.4459 in", "tapDrillDiameter" : "2.4459 in", "cBoreDiameter" : "3.875 in", "cBoreDepth" : "2.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.5 in"},
                        "75%" : {"holeDiameter" : "2.435 in", "tapDrillDiameter" : "2.435 in", "cBoreDiameter" : "3.875 in", "cBoreDepth" : "2.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.5 in"}
                    }
                }
            }
        },
        "2 3/4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "4 tpi",
            "entries" : {
                "4 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "2.5876 in", "tapDrillDiameter" : "2.5876 in", "cBoreDiameter" : "4.25 in", "cBoreDepth" : "2.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.75 in"},
                        "75%" : {"holeDiameter" : "2.5075 in", "tapDrillDiameter" : "2.5075 in", "cBoreDiameter" : "4.25 in", "cBoreDepth" : "2.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.75 in"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "2.6959 in", "tapDrillDiameter" : "2.6959 in", "cBoreDiameter" : "4.25 in", "cBoreDepth" : "2.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.75 in"},
                        "75%" : {"holeDiameter" : "2.6735 in", "tapDrillDiameter" : "2.6735 in", "cBoreDiameter" : "4.25 in", "cBoreDepth" : "2.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.75 in"}
                    }
                }
            }
        },
        "3" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "4 tpi",
            "entries" : {
                "4 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "2.8376 in", "tapDrillDiameter" : "2.8376 in", "cBoreDiameter" : "4.625 in", "cBoreDepth" : "3 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3 in"},
                        "75%" : {"holeDiameter" : "2.7575 in", "tapDrillDiameter" : "2.7575 in", "cBoreDiameter" : "4.625 in", "cBoreDepth" : "3 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3 in"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "2.9459 in", "tapDrillDiameter" : "2.9459 in", "cBoreDiameter" : "4.625 in", "cBoreDepth" : "3 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3 in"},
                        "75%" : {"holeDiameter" : "2.9235 in", "tapDrillDiameter" : "2.9235 in", "cBoreDiameter" : "4.625 in", "cBoreDepth" : "3 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3 in"}
                    }
                }
            }
        },
        "3 1/4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "4 tpi",
            "entries" : {
                "4 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "3.0876 in", "tapDrillDiameter" : "3.0876 in", "cBoreDiameter" : "5 in", "cBoreDepth" : "3.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.25 in"},
                        "75%" : {"holeDiameter" : "3.0075 in", "tapDrillDiameter" : "3.0075 in", "cBoreDiameter" : "5 in", "cBoreDepth" : "3.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.25 in"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "3.1959 in", "tapDrillDiameter" : "3.1959 in", "cBoreDiameter" : "5 in", "cBoreDepth" : "3.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.25 in"},
                        "75%" : {"holeDiameter" : "3.1735 in", "tapDrillDiameter" : "3.1735 in", "cBoreDiameter" : "5 in", "cBoreDepth" : "3.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.25 in"}
                    }
                }
            }
        },
        "3 1/2" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "4 tpi",
            "entries" : {
                "4 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "3.3376 in", "tapDrillDiameter" : "3.3376 in", "cBoreDiameter" : "5.375 in", "cBoreDepth" : "3.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.5 in"},
                        "75%" : {"holeDiameter" : "3.2575 in", "tapDrillDiameter" : "3.2575 in", "cBoreDiameter" : "5.375 in", "cBoreDepth" : "3.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.5 in"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "3.4459 in", "tapDrillDiameter" : "3.4459 in", "cBoreDiameter" : "5.375 in", "cBoreDepth" : "3.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.5 in"},
                        "75%" : {"holeDiameter" : "3.4235 in", "tapDrillDiameter" : "3.4235 in", "cBoreDiameter" : "5.375 in", "cBoreDepth" : "3.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.5 in"}
                    }
                }
            }
        },
        "3 3/4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "4 tpi",
            "entries" : {
                "4 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "3.5876 in", "tapDrillDiameter" : "3.5876 in", "cBoreDiameter" : "5.75 in", "cBoreDepth" : "3.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.75 in"},
                        "75%" : {"holeDiameter" : "3.5075 in", "tapDrillDiameter" : "3.5075 in", "cBoreDiameter" : "5.75 in", "cBoreDepth" : "3.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.75 in"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "3.6959 in", "tapDrillDiameter" : "3.6959 in", "cBoreDiameter" : "5.75 in", "cBoreDepth" : "3.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.75 in"},
                        "75%" : {"holeDiameter" : "3.6735 in", "tapDrillDiameter" : "3.6735 in", "cBoreDiameter" : "5.75 in", "cBoreDepth" : "3.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.75 in"}
                    }
                }
            }
        },
        "4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "4 tpi",
            "entries" : {
                "4 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "3.8376 in", "tapDrillDiameter" : "3.8376 in", "cBoreDiameter" : "6.125 in", "cBoreDepth" : "4 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "4 in"},
                        "75%" : {"holeDiameter" : "3.7575 in", "tapDrillDiameter" : "3.7575 in", "cBoreDiameter" : "6.125 in", "cBoreDepth" : "4 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "4 in"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "3.9459 in", "tapDrillDiameter" : "3.9459 in", "cBoreDiameter" : "6.125 in", "cBoreDepth" : "4 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "4 in"},
                        "75%" : {"holeDiameter" : "3.9235 in", "tapDrillDiameter" : "3.9235 in", "cBoreDiameter" : "6.125 in", "cBoreDepth" : "4 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "4 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.052 in", "holeDiameter" : "0.0635 in", "cBoreDiameter" : "1/8 in", "cBoreDepth" : "0.06 in", "cSinkDiameter" : "0.138 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.06 in"},
                                "75%" : {"tapDrillDiameter" : "0.0469 in", "holeDiameter" : "0.0635 in", "cBoreDiameter" : "1/8 in", "cBoreDepth" : "0.06 in", "cSinkDiameter" : "0.138 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.06 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.052 in", "holeDiameter" : "0.07 in", "cBoreDiameter" : "1/8 in", "cBoreDepth" : "0.06 in", "cSinkDiameter" : "0.138 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.06 in"},
                                "75%" : {"tapDrillDiameter" : "0.0469 in", "holeDiameter" : "0.07 in", "cBoreDiameter" : "1/8 in", "cBoreDepth" : "0.06 in", "cSinkDiameter" : "0.138 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.06 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.052 in", "holeDiameter" : "0.067 in", "cBoreDiameter" : "1/8 in", "cBoreDepth" : "0.06 in", "cSinkDiameter" : "0.138 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.06 in"},
                                "75%" : {"tapDrillDiameter" : "0.0469 in", "holeDiameter" : "0.067 in", "cBoreDiameter" : "1/8 in", "cBoreDepth" : "0.06 in", "cSinkDiameter" : "0.138 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.06 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.052 in", "holeDiameter" : "0.076 in", "cBoreDiameter" : "1/8 in", "cBoreDepth" : "0.06 in", "cSinkDiameter" : "0.138 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.06 in"},
                                "75%" : {"tapDrillDiameter" : "0.0469 in", "holeDiameter" : "0.076 in", "cBoreDiameter" : "1/8 in", "cBoreDepth" : "0.06 in", "cSinkDiameter" : "0.138 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.06 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.052 in", "holeDiameter" : "0.094 in", "cBoreDiameter" : "1/8 in", "cBoreDepth" : "0.06 in", "cSinkDiameter" : "0.138 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.06 in"},
                                "75%" : {"tapDrillDiameter" : "0.0469 in", "holeDiameter" : "0.094 in", "cBoreDiameter" : "1/8 in", "cBoreDepth" : "0.06 in", "cSinkDiameter" : "0.138 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.06 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.0625 in", "holeDiameter" : "0.076 in", "cBoreDiameter" : "5/32 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.073 in"},
                                "75%" : {"tapDrillDiameter" : "0.0595 in", "holeDiameter" : "0.076 in", "cBoreDiameter" : "5/32 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.073 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.0625 in", "holeDiameter" : "0.081 in", "cBoreDiameter" : "5/32 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.073 in"},
                                "75%" : {"tapDrillDiameter" : "0.0595 in", "holeDiameter" : "0.081 in", "cBoreDiameter" : "5/32 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.073 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.0625 in", "holeDiameter" : "0.081 in", "cBoreDiameter" : "5/32 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.073 in"},
                                "75%" : {"tapDrillDiameter" : "0.0595 in", "holeDiameter" : "0.081 in", "cBoreDiameter" : "5/32 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.073 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.0625 in", "holeDiameter" : "0.089 in", "cBoreDiameter" : "5/32 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.073 in"},
                                "75%" : {"tapDrillDiameter" : "0.0595 in", "holeDiameter" : "0.089 in", "cBoreDiameter" : "5/32 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.073 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.0625 in", "holeDiameter" : "0.104 in", "cBoreDiameter" : "5/32 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.073 in"},
                                "75%" : {"tapDrillDiameter" : "0.0595 in", "holeDiameter" : "0.104 in", "cBoreDiameter" : "5/32 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.073 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.0635 in", "holeDiameter" : "0.076 in", "cBoreDiameter" : "5/33 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.073 in"},
                                "75%" : {"tapDrillDiameter" : "0.0595 in", "holeDiameter" : "0.076 in", "cBoreDiameter" : "5/33 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.073 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.0635 in", "holeDiameter" : "0.081 in", "cBoreDiameter" : "5/33 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.073 in"},
                                "75%" : {"tapDrillDiameter" : "0.0595 in", "holeDiameter" : "0.081 in", "cBoreDiameter" : "5/33 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.073 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.0635 in", "holeDiameter" : "0.081 in", "cBoreDiameter" : "5/33 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.073 in"},
                                "75%" : {"tapDrillDiameter" : "0.0595 in", "holeDiameter" : "0.081 in", "cBoreDiameter" : "5/33 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.073 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.0635 in", "holeDiameter" : "0.089 in", "cBoreDiameter" : "5/33 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.073 in"},
                                "75%" : {"tapDrillDiameter" : "0.0595 in", "holeDiameter" : "0.089 in", "cBoreDiameter" : "5/33 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.073 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.0635 in", "holeDiameter" : "0.104 in", "cBoreDiameter" : "5/33 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.073 in"},
                                "75%" : {"tapDrillDiameter" : "0.0595 in", "holeDiameter" : "0.104 in", "cBoreDiameter" : "5/33 in", "cBoreDepth" : "0.073 in", "cSinkDiameter" : "0.168 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.073 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.073 in", "holeDiameter" : "0.089 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.086 in"},
                                "75%" : {"tapDrillDiameter" : "0.07 in", "holeDiameter" : "0.089 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.086 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.073 in", "holeDiameter" : "0.096 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.086 in"},
                                "75%" : {"tapDrillDiameter" : "0.07 in", "holeDiameter" : "0.096 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.086 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.073 in", "holeDiameter" : "0.094 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.086 in"},
                                "75%" : {"tapDrillDiameter" : "0.07 in", "holeDiameter" : "0.094 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.086 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.073 in", "holeDiameter" : "0.102 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.086 in"},
                                "75%" : {"tapDrillDiameter" : "0.07 in", "holeDiameter" : "0.102 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.086 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.073 in", "holeDiameter" : "0.116 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.086 in"},
                                "75%" : {"tapDrillDiameter" : "0.07 in", "holeDiameter" : "0.116 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.086 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.076 in", "holeDiameter" : "0.089 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.086 in"},
                                "75%" : {"tapDrillDiameter" : "0.07 in", "holeDiameter" : "0.089 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.086 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.076 in", "holeDiameter" : "0.096 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.086 in"},
                                "75%" : {"tapDrillDiameter" : "0.07 in", "holeDiameter" : "0.096 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.086 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.076 in", "holeDiameter" : "0.094 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.086 in"},
                                "75%" : {"tapDrillDiameter" : "0.07 in", "holeDiameter" : "0.094 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.086 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.076 in", "holeDiameter" : "0.102 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.086 in"},
                                "75%" : {"tapDrillDiameter" : "0.07 in", "holeDiameter" : "0.102 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.086 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.076 in", "holeDiameter" : "0.116 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.086 in"},
                                "75%" : {"tapDrillDiameter" : "0.07 in", "holeDiameter" : "0.116 in", "cBoreDiameter" : "3/16 in", "cBoreDepth" : "0.086 in", "cSinkDiameter" : "0.197 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.086 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.086 in", "holeDiameter" : "0.104 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.099 in"},
                                "75%" : {"tapDrillDiameter" : "0.0785 in", "holeDiameter" : "0.104 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.099 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.086 in", "holeDiameter" : "0.11 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.099 in"},
                                "75%" : {"tapDrillDiameter" : "0.0785 in", "holeDiameter" : "0.11 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.099 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.086 in", "holeDiameter" : "0.106 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.099 in"},
                                "75%" : {"tapDrillDiameter" : "0.0785 in", "holeDiameter" : "0.106 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.099 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.086 in", "holeDiameter" : "0.116 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.099 in"},
                                "75%" : {"tapDrillDiameter" : "0.0785 in", "holeDiameter" : "0.116 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.099 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.086 in", "holeDiameter" : "0.128 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.099 in"},
                                "75%" : {"tapDrillDiameter" : "0.0785 in", "holeDiameter" : "0.128 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.099 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.089 in", "holeDiameter" : "0.104 in", "cBoreDiameter" : "7/33 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.099 in"},
                                "75%" : {"tapDrillDiameter" : "0.082 in", "holeDiameter" : "0.104 in", "cBoreDiameter" : "7/33 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.099 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.089 in", "holeDiameter" : "0.11 in", "cBoreDiameter" : "7/33 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.099 in"},
                                "75%" : {"tapDrillDiameter" : "0.082 in", "holeDiameter" : "0.11 in", "cBoreDiameter" : "7/33 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.099 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.089 in", "holeDiameter" : "0.106 in", "cBoreDiameter" : "7/33 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.099 in"},
                                "75%" : {"tapDrillDiameter" : "0.082 in", "holeDiameter" : "0.106 in", "cBoreDiameter" : "7/33 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.099 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.089 in", "holeDiameter" : "0.116 in", "cBoreDiameter" : "7/33 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.099 in"},
                                "75%" : {"tapDrillDiameter" : "0.082 in", "holeDiameter" : "0.116 in", "cBoreDiameter" : "7/33 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.099 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.089 in", "holeDiameter" : "0.128 in", "cBoreDiameter" : "7/33 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.099 in"},
                                "75%" : {"tapDrillDiameter" : "0.082 in", "holeDiameter" : "0.128 in", "cBoreDiameter" : "7/33 in", "cBoreDepth" : "0.099 in", "cSinkDiameter" : "0.226 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.099 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.096 in", "holeDiameter" : "0.116 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.112 in"},
                                "75%" : {"tapDrillDiameter" : "0.089 in", "holeDiameter" : "0.116 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.112 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.096 in", "holeDiameter" : "0.1285 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.112 in"},
                                "75%" : {"tapDrillDiameter" : "0.089 in", "holeDiameter" : "0.1285 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.112 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.096 in", "holeDiameter" : "0.12 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.112 in"},
                                "75%" : {"tapDrillDiameter" : "0.089 in", "holeDiameter" : "0.12 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.112 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.096 in", "holeDiameter" : "0.128 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.112 in"},
                                "75%" : {"tapDrillDiameter" : "0.089 in", "holeDiameter" : "0.128 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.112 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.096 in", "holeDiameter" : "0.144 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.112 in"},
                                "75%" : {"tapDrillDiameter" : "0.089 in", "holeDiameter" : "0.144 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.112 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.098 in", "holeDiameter" : "0.116 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.112 in"},
                                "75%" : {"tapDrillDiameter" : "0.0935 in", "holeDiameter" : "0.116 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.112 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.098 in", "holeDiameter" : "0.1285 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.112 in"},
                                "75%" : {"tapDrillDiameter" : "0.0935 in", "holeDiameter" : "0.1285 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.112 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.098 in", "holeDiameter" : "0.12 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.112 in"},
                                "75%" : {"tapDrillDiameter" : "0.0935 in", "holeDiameter" : "0.12 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.112 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.098 in", "holeDiameter" : "0.128 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.112 in"},
                                "75%" : {"tapDrillDiameter" : "0.0935 in", "holeDiameter" : "0.128 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.112 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.098 in", "holeDiameter" : "0.144 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.112 in"},
                                "75%" : {"tapDrillDiameter" : "0.0935 in", "holeDiameter" : "0.144 in", "cBoreDiameter" : "7/32 in", "cBoreDepth" : "0.112 in", "cSinkDiameter" : "0.255 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.112 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.1094 in", "holeDiameter" : "0.1285 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.125 in"},
                                "75%" : {"tapDrillDiameter" : "0.1015 in", "holeDiameter" : "0.1285 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.125 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.1094 in", "holeDiameter" : "0.136 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.125 in"},
                                "75%" : {"tapDrillDiameter" : "0.1015 in", "holeDiameter" : "0.136 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.125 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.1094 in", "holeDiameter" : "0.141 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.125 in"},
                                "75%" : {"tapDrillDiameter" : "0.1015 in", "holeDiameter" : "0.141 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.125 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.1094 in", "holeDiameter" : "0.156 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.125 in"},
                                "75%" : {"tapDrillDiameter" : "0.1015 in", "holeDiameter" : "0.156 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.125 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.1094 in", "holeDiameter" : "0.172 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.125 in"},
                                "75%" : {"tapDrillDiameter" : "0.1015 in", "holeDiameter" : "0.172 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.125 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.11 in", "holeDiameter" : "0.1285 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.125 in"},
                                "75%" : {"tapDrillDiameter" : "0.104 in", "holeDiameter" : "0.1285 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.125 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.11 in", "holeDiameter" : "0.136 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.125 in"},
                                "75%" : {"tapDrillDiameter" : "0.104 in", "holeDiameter" : "0.136 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.125 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.11 in", "holeDiameter" : "0.141 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.125 in"},
                                "75%" : {"tapDrillDiameter" : "0.104 in", "holeDiameter" : "0.141 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.125 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.11 in", "holeDiameter" : "0.156 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.125 in"},
                                "75%" : {"tapDrillDiameter" : "0.104 in", "holeDiameter" : "0.156 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.125 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.11 in", "holeDiameter" : "0.172 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.125 in"},
                                "75%" : {"tapDrillDiameter" : "0.104 in", "holeDiameter" : "0.172 in", "cBoreDiameter" : "1/4 in", "cBoreDepth" : "0.125 in", "cSinkDiameter" : "0.281 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.125 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.116 in", "holeDiameter" : "0.144 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.138 in"},
                                "75%" : {"tapDrillDiameter" : "0.1065 in", "holeDiameter" : "0.144 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.138 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.116 in", "holeDiameter" : "0.1495 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.138 in"},
                                "75%" : {"tapDrillDiameter" : "0.1065 in", "holeDiameter" : "0.1495 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.138 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.116 in", "holeDiameter" : "0.154 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.138 in"},
                                "75%" : {"tapDrillDiameter" : "0.1065 in", "holeDiameter" : "0.154 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.138 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.116 in", "holeDiameter" : "0.17 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.138 in"},
                                "75%" : {"tapDrillDiameter" : "0.1065 in", "holeDiameter" : "0.17 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.138 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.116 in", "holeDiameter" : "0.185 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.138 in"},
                                "75%" : {"tapDrillDiameter" : "0.1065 in", "holeDiameter" : "0.185 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.138 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.12 in", "holeDiameter" : "0.144 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.138 in"},
                                "75%" : {"tapDrillDiameter" : "0.113 in", "holeDiameter" : "0.144 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.138 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.12 in", "holeDiameter" : "0.1495 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.138 in"},
                                "75%" : {"tapDrillDiameter" : "0.113 in", "holeDiameter" : "0.1495 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.138 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.12 in", "holeDiameter" : "0.154 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.138 in"},
                                "75%" : {"tapDrillDiameter" : "0.113 in", "holeDiameter" : "0.154 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.138 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.12 in", "holeDiameter" : "0.17 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.138 in"},
                                "75%" : {"tapDrillDiameter" : "0.113 in", "holeDiameter" : "0.17 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.138 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.12 in", "holeDiameter" : "0.185 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.138 in"},
                                "75%" : {"tapDrillDiameter" : "0.113 in", "holeDiameter" : "0.185 in", "cBoreDiameter" : "9/32 in", "cBoreDepth" : "0.138 in", "cSinkDiameter" : "0.307 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.138 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.144 in", "holeDiameter" : "0.1695 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.164 in"},
                                "75%" : {"tapDrillDiameter" : "0.136 in", "holeDiameter" : "0.1695 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.164 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.144 in", "holeDiameter" : "0.177 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.164 in"},
                                "75%" : {"tapDrillDiameter" : "0.136 in", "holeDiameter" : "0.177 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.164 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.144 in", "holeDiameter" : "0.18 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.164 in"},
                                "75%" : {"tapDrillDiameter" : "0.136 in", "holeDiameter" : "0.18 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.164 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.144 in", "holeDiameter" : "0.196 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.164 in"},
                                "75%" : {"tapDrillDiameter" : "0.136 in", "holeDiameter" : "0.196 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.164 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.144 in", "holeDiameter" : "0.213 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.164 in"},
                                "75%" : {"tapDrillDiameter" : "0.136 in", "holeDiameter" : "0.213 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.164 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.147 in", "holeDiameter" : "0.1695 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.164 in"},
                                "75%" : {"tapDrillDiameter" : "0.136 in", "holeDiameter" : "0.1695 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.164 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.147 in", "holeDiameter" : "0.177 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.164 in"},
                                "75%" : {"tapDrillDiameter" : "0.136 in", "holeDiameter" : "0.177 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.164 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.147 in", "holeDiameter" : "0.18 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.164 in"},
                                "75%" : {"tapDrillDiameter" : "0.136 in", "holeDiameter" : "0.18 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.164 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.147 in", "holeDiameter" : "0.196 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.164 in"},
                                "75%" : {"tapDrillDiameter" : "0.136 in", "holeDiameter" : "0.196 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.164 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.147 in", "holeDiameter" : "0.213 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.164 in"},
                                "75%" : {"tapDrillDiameter" : "0.136 in", "holeDiameter" : "0.213 in", "cBoreDiameter" : "5/16 in", "cBoreDepth" : "0.164 in", "cSinkDiameter" : "0.359 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.164 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.161 in", "holeDiameter" : "0.196 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.19 in"},
                                "75%" : {"tapDrillDiameter" : "0.1495 in", "holeDiameter" : "0.196 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.19 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.161 in", "holeDiameter" : "0.201 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.19 in"},
                                "75%" : {"tapDrillDiameter" : "0.1495 in", "holeDiameter" : "0.201 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.19 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.161 in", "holeDiameter" : "0.206 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.19 in"},
                                "75%" : {"tapDrillDiameter" : "0.1495 in", "holeDiameter" : "0.206 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.19 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.161 in", "holeDiameter" : "0.221 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.19 in"},
                                "75%" : {"tapDrillDiameter" : "0.1495 in", "holeDiameter" : "0.221 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.19 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.161 in", "holeDiameter" : "0.238 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.19 in"},
                                "75%" : {"tapDrillDiameter" : "0.1495 in", "holeDiameter" : "0.238 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.19 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.1695 in", "holeDiameter" : "0.196 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.19 in"},
                                "75%" : {"tapDrillDiameter" : "0.159 in", "holeDiameter" : "0.196 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.19 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.1695 in", "holeDiameter" : "0.201 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.19 in"},
                                "75%" : {"tapDrillDiameter" : "0.159 in", "holeDiameter" : "0.201 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.19 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.1695 in", "holeDiameter" : "0.206 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.19 in"},
                                "75%" : {"tapDrillDiameter" : "0.159 in", "holeDiameter" : "0.206 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.19 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.1695 in", "holeDiameter" : "0.221 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.19 in"},
                                "75%" : {"tapDrillDiameter" : "0.159 in", "holeDiameter" : "0.221 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.19 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.1695 in", "holeDiameter" : "0.238 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.19 in"},
                                "75%" : {"tapDrillDiameter" : "0.159 in", "holeDiameter" : "0.238 in", "cBoreDiameter" : "3/8 in", "cBoreDepth" : "0.19 in", "cSinkDiameter" : "0.411 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.19 in"}
                            }
                        }
                    }
                }
            }
        },
        "#12" : {
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
                                "50%" : {"tapDrillDiameter" : "0.189 in", "holeDiameter" : "0.221 in", "cBoreDiameter" : "13/32 in", "cBoreDepth" : "0.216 in", "cSinkDiameter" : "0.45 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.216 in"},
                                "75%" : {"tapDrillDiameter" : "0.177 in", "holeDiameter" : "0.221 in", "cBoreDiameter" : "13/32 in", "cBoreDepth" : "0.216 in", "cSinkDiameter" : "0.45 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.216 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.189 in", "holeDiameter" : "0.228 in", "cBoreDiameter" : "13/32 in", "cBoreDepth" : "0.216 in", "cSinkDiameter" : "0.45 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.216 in"},
                                "75%" : {"tapDrillDiameter" : "0.177 in", "holeDiameter" : "0.228 in", "cBoreDiameter" : "13/32 in", "cBoreDepth" : "0.216 in", "cSinkDiameter" : "0.45 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.216 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.1935 in", "holeDiameter" : "0.221 in", "cBoreDiameter" : "13/32 in", "cBoreDepth" : "0.216 in", "cSinkDiameter" : "0.45 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.216 in"},
                                "75%" : {"tapDrillDiameter" : "0.182 in", "holeDiameter" : "0.221 in", "cBoreDiameter" : "13/32 in", "cBoreDepth" : "0.216 in", "cSinkDiameter" : "0.45 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.216 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.1935 in", "holeDiameter" : "0.228 in", "cBoreDiameter" : "13/32 in", "cBoreDepth" : "0.216 in", "cSinkDiameter" : "0.45 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.216 in"},
                                "75%" : {"tapDrillDiameter" : "0.182 in", "holeDiameter" : "0.228 in", "cBoreDiameter" : "13/32 in", "cBoreDepth" : "0.216 in", "cSinkDiameter" : "0.45 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.216 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.196 in", "holeDiameter" : "0.221 in", "cBoreDiameter" : "13/32 in", "cBoreDepth" : "0.216 in", "cSinkDiameter" : "0.45 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.216 in"},
                                "75%" : {"tapDrillDiameter" : "0.185 in", "holeDiameter" : "0.221 in", "cBoreDiameter" : "13/32 in", "cBoreDepth" : "0.216 in", "cSinkDiameter" : "0.45 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.216 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.196 in", "holeDiameter" : "0.228 in", "cBoreDiameter" : "13/32 in", "cBoreDepth" : "0.216 in", "cSinkDiameter" : "0.45 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.216 in"},
                                "75%" : {"tapDrillDiameter" : "0.185 in", "holeDiameter" : "0.228 in", "cBoreDiameter" : "13/32 in", "cBoreDepth" : "0.216 in", "cSinkDiameter" : "0.45 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.216 in"}
                            }
                        }
                    }
                }
            }
        },
        "1/4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "20 tpi",
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
                                "50%" : {"tapDrillDiameter" : "0.2188 in", "holeDiameter" : "0.257 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"},
                                "75%" : {"tapDrillDiameter" : "0.201 in", "holeDiameter" : "0.257 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.2188 in", "holeDiameter" : "0.266 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"},
                                "75%" : {"tapDrillDiameter" : "0.201 in", "holeDiameter" : "0.266 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.2188 in", "holeDiameter" : "0.266 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"},
                                "75%" : {"tapDrillDiameter" : "0.201 in", "holeDiameter" : "0.266 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.2188 in", "holeDiameter" : "0.281 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"},
                                "75%" : {"tapDrillDiameter" : "0.201 in", "holeDiameter" : "0.281 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.2188 in", "holeDiameter" : "0.297 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"},
                                "75%" : {"tapDrillDiameter" : "0.201 in", "holeDiameter" : "0.297 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.228 in", "holeDiameter" : "0.257 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"},
                                "75%" : {"tapDrillDiameter" : "0.213 in", "holeDiameter" : "0.257 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.228 in", "holeDiameter" : "0.266 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"},
                                "75%" : {"tapDrillDiameter" : "0.213 in", "holeDiameter" : "0.266 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.228 in", "holeDiameter" : "0.266 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"},
                                "75%" : {"tapDrillDiameter" : "0.213 in", "holeDiameter" : "0.266 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.228 in", "holeDiameter" : "0.281 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"},
                                "75%" : {"tapDrillDiameter" : "0.213 in", "holeDiameter" : "0.281 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.228 in", "holeDiameter" : "0.297 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"},
                                "75%" : {"tapDrillDiameter" : "0.213 in", "holeDiameter" : "0.297 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.228 in", "holeDiameter" : "0.257 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"},
                                "75%" : {"tapDrillDiameter" : "0.2188 in", "holeDiameter" : "0.257 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.228 in", "holeDiameter" : "0.266 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"},
                                "75%" : {"tapDrillDiameter" : "0.2188 in", "holeDiameter" : "0.266 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.228 in", "holeDiameter" : "0.266 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"},
                                "75%" : {"tapDrillDiameter" : "0.2188 in", "holeDiameter" : "0.266 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.228 in", "holeDiameter" : "0.281 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"},
                                "75%" : {"tapDrillDiameter" : "0.2188 in", "holeDiameter" : "0.281 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.228 in", "holeDiameter" : "0.297 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"},
                                "75%" : {"tapDrillDiameter" : "0.2188 in", "holeDiameter" : "0.297 in", "cBoreDiameter" : "7/16 in", "cBoreDepth" : "0.25 in", "cSinkDiameter" : "0.531 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.25 in"}
                            }
                        }
                    }
                }
            }
        },
        "5/16" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "18 tpi",
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
                                "50%" : {"tapDrillDiameter" : "0.277 in", "holeDiameter" : "0.323 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"},
                                "75%" : {"tapDrillDiameter" : "0.257 in", "holeDiameter" : "0.323 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.277 in", "holeDiameter" : "0.332 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"},
                                "75%" : {"tapDrillDiameter" : "0.257 in", "holeDiameter" : "0.332 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.277 in", "holeDiameter" : "0.328 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"},
                                "75%" : {"tapDrillDiameter" : "0.257 in", "holeDiameter" : "0.328 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.277 in", "holeDiameter" : "0.344 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"},
                                "75%" : {"tapDrillDiameter" : "0.257 in", "holeDiameter" : "0.344 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.277 in", "holeDiameter" : "0.359 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"},
                                "75%" : {"tapDrillDiameter" : "0.257 in", "holeDiameter" : "0.359 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.2812 in", "holeDiameter" : "0.323 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"},
                                "75%" : {"tapDrillDiameter" : "0.272 in", "holeDiameter" : "0.323 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.2812 in", "holeDiameter" : "0.332 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"},
                                "75%" : {"tapDrillDiameter" : "0.272 in", "holeDiameter" : "0.332 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.2812 in", "holeDiameter" : "0.328 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"},
                                "75%" : {"tapDrillDiameter" : "0.272 in", "holeDiameter" : "0.328 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.2812 in", "holeDiameter" : "0.344 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"},
                                "75%" : {"tapDrillDiameter" : "0.272 in", "holeDiameter" : "0.344 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.2812 in", "holeDiameter" : "0.359 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"},
                                "75%" : {"tapDrillDiameter" : "0.272 in", "holeDiameter" : "0.359 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.29 in", "holeDiameter" : "0.323 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"},
                                "75%" : {"tapDrillDiameter" : "0.2812 in", "holeDiameter" : "0.323 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.29 in", "holeDiameter" : "0.332 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"},
                                "75%" : {"tapDrillDiameter" : "0.2812 in", "holeDiameter" : "0.332 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.29 in", "holeDiameter" : "0.328 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"},
                                "75%" : {"tapDrillDiameter" : "0.2812 in", "holeDiameter" : "0.328 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.29 in", "holeDiameter" : "0.344 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"},
                                "75%" : {"tapDrillDiameter" : "0.2812 in", "holeDiameter" : "0.344 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.29 in", "holeDiameter" : "0.359 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"},
                                "75%" : {"tapDrillDiameter" : "0.2812 in", "holeDiameter" : "0.359 in", "cBoreDiameter" : "17/32 in", "cBoreDepth" : "0.3125 in", "cSinkDiameter" : "0.656 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.3125 in"}
                            }
                        }
                    }
                }
            }
        },
        "3/8" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "16 tpi",
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
                                "50%" : {"tapDrillDiameter" : "0.332 in", "holeDiameter" : "0.386 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"},
                                "75%" : {"tapDrillDiameter" : "0.3125 in", "holeDiameter" : "0.386 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.332 in", "holeDiameter" : "0.397 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"},
                                "75%" : {"tapDrillDiameter" : "0.3125 in", "holeDiameter" : "0.397 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.332 in", "holeDiameter" : "0.391 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"},
                                "75%" : {"tapDrillDiameter" : "0.3125 in", "holeDiameter" : "0.391 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.332 in", "holeDiameter" : "0.406 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"},
                                "75%" : {"tapDrillDiameter" : "0.3125 in", "holeDiameter" : "0.406 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.332 in", "holeDiameter" : "0.422 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"},
                                "75%" : {"tapDrillDiameter" : "0.3125 in", "holeDiameter" : "0.422 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.348 in", "holeDiameter" : "0.386 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"},
                                "75%" : {"tapDrillDiameter" : "0.332 in", "holeDiameter" : "0.386 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.348 in", "holeDiameter" : "0.397 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"},
                                "75%" : {"tapDrillDiameter" : "0.332 in", "holeDiameter" : "0.397 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.348 in", "holeDiameter" : "0.391 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"},
                                "75%" : {"tapDrillDiameter" : "0.332 in", "holeDiameter" : "0.391 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.348 in", "holeDiameter" : "0.406 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"},
                                "75%" : {"tapDrillDiameter" : "0.332 in", "holeDiameter" : "0.406 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.348 in", "holeDiameter" : "0.422 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"},
                                "75%" : {"tapDrillDiameter" : "0.332 in", "holeDiameter" : "0.422 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.358 in", "holeDiameter" : "0.386 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"},
                                "75%" : {"tapDrillDiameter" : "0.3438 in", "holeDiameter" : "0.386 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.358 in", "holeDiameter" : "0.397 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"},
                                "75%" : {"tapDrillDiameter" : "0.3438 in", "holeDiameter" : "0.397 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.358 in", "holeDiameter" : "0.391 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"},
                                "75%" : {"tapDrillDiameter" : "0.3438 in", "holeDiameter" : "0.391 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.358 in", "holeDiameter" : "0.406 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"},
                                "75%" : {"tapDrillDiameter" : "0.3438 in", "holeDiameter" : "0.406 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.358 in", "holeDiameter" : "0.422 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"},
                                "75%" : {"tapDrillDiameter" : "0.3438 in", "holeDiameter" : "0.422 in", "cBoreDiameter" : "5/8 in", "cBoreDepth" : "0.375 in", "cSinkDiameter" : "0.781 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.375 in"}
                            }
                        }
                    }
                }
            }
        },
        "7/16" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "14 tpi",
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
                                "50%" : {"tapDrillDiameter" : "0.3906 in", "holeDiameter" : "0.4531 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"},
                                "75%" : {"tapDrillDiameter" : "0.368 in", "holeDiameter" : "0.4531 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.3906 in", "holeDiameter" : "0.4687 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"},
                                "75%" : {"tapDrillDiameter" : "0.368 in", "holeDiameter" : "0.4687 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.3906 in", "holeDiameter" : "0.453 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"},
                                "75%" : {"tapDrillDiameter" : "0.368 in", "holeDiameter" : "0.453 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.3906 in", "holeDiameter" : "0.469 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"},
                                "75%" : {"tapDrillDiameter" : "0.368 in", "holeDiameter" : "0.469 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.3906 in", "holeDiameter" : "0.484 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"},
                                "75%" : {"tapDrillDiameter" : "0.368 in", "holeDiameter" : "0.484 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.4062 in", "holeDiameter" : "0.4531 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"},
                                "75%" : {"tapDrillDiameter" : "0.3906 in", "holeDiameter" : "0.4531 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4062 in", "holeDiameter" : "0.4687 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"},
                                "75%" : {"tapDrillDiameter" : "0.3906 in", "holeDiameter" : "0.4687 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4062 in", "holeDiameter" : "0.453 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"},
                                "75%" : {"tapDrillDiameter" : "0.3906 in", "holeDiameter" : "0.453 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4062 in", "holeDiameter" : "0.469 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"},
                                "75%" : {"tapDrillDiameter" : "0.3906 in", "holeDiameter" : "0.469 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4062 in", "holeDiameter" : "0.484 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"},
                                "75%" : {"tapDrillDiameter" : "0.3906 in", "holeDiameter" : "0.484 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.413 in", "holeDiameter" : "0.4531 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"},
                                "75%" : {"tapDrillDiameter" : "0.404 in", "holeDiameter" : "0.4531 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.413 in", "holeDiameter" : "0.4687 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"},
                                "75%" : {"tapDrillDiameter" : "0.404 in", "holeDiameter" : "0.4687 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.413 in", "holeDiameter" : "0.453 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"},
                                "75%" : {"tapDrillDiameter" : "0.404 in", "holeDiameter" : "0.453 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.413 in", "holeDiameter" : "0.469 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"},
                                "75%" : {"tapDrillDiameter" : "0.404 in", "holeDiameter" : "0.469 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.413 in", "holeDiameter" : "0.484 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"},
                                "75%" : {"tapDrillDiameter" : "0.404 in", "holeDiameter" : "0.484 in", "cBoreDiameter" : "23/32 in", "cBoreDepth" : "0.4375 in", "cSinkDiameter" : "0.844 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.4375 in"}
                            }
                        }
                    }
                }
            }
        },
        "1/2" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "13 tpi",
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
                                "50%" : {"tapDrillDiameter" : "0.4531 in", "holeDiameter" : "0.5156 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"},
                                "75%" : {"tapDrillDiameter" : "0.4219 in", "holeDiameter" : "0.5156 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4531 in", "holeDiameter" : "0.5312 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"},
                                "75%" : {"tapDrillDiameter" : "0.4219 in", "holeDiameter" : "0.5312 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4531 in", "holeDiameter" : "0.531 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"},
                                "75%" : {"tapDrillDiameter" : "0.4219 in", "holeDiameter" : "0.531 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4531 in", "holeDiameter" : "0.562 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"},
                                "75%" : {"tapDrillDiameter" : "0.4219 in", "holeDiameter" : "0.562 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4531 in", "holeDiameter" : "0.609 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"},
                                "75%" : {"tapDrillDiameter" : "0.4219 in", "holeDiameter" : "0.609 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.4688 in", "holeDiameter" : "0.5156 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"},
                                "75%" : {"tapDrillDiameter" : "0.4531 in", "holeDiameter" : "0.5156 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4688 in", "holeDiameter" : "0.5312 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"},
                                "75%" : {"tapDrillDiameter" : "0.4531 in", "holeDiameter" : "0.5312 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4688 in", "holeDiameter" : "0.531 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"},
                                "75%" : {"tapDrillDiameter" : "0.4531 in", "holeDiameter" : "0.531 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4688 in", "holeDiameter" : "0.562 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"},
                                "75%" : {"tapDrillDiameter" : "0.4531 in", "holeDiameter" : "0.562 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4688 in", "holeDiameter" : "0.609 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"},
                                "75%" : {"tapDrillDiameter" : "0.4531 in", "holeDiameter" : "0.609 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.4688 in", "holeDiameter" : "0.5156 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"},
                                "75%" : {"tapDrillDiameter" : "0.4688 in", "holeDiameter" : "0.5156 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4688 in", "holeDiameter" : "0.5312 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"},
                                "75%" : {"tapDrillDiameter" : "0.4688 in", "holeDiameter" : "0.5312 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4688 in", "holeDiameter" : "0.531 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"},
                                "75%" : {"tapDrillDiameter" : "0.4688 in", "holeDiameter" : "0.531 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4688 in", "holeDiameter" : "0.562 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"},
                                "75%" : {"tapDrillDiameter" : "0.4688 in", "holeDiameter" : "0.562 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4688 in", "holeDiameter" : "0.609 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"},
                                "75%" : {"tapDrillDiameter" : "0.4688 in", "holeDiameter" : "0.609 in", "cBoreDiameter" : "13/16 in", "cBoreDepth" : "0.5 in", "cSinkDiameter" : "0.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5 in"}
                            }
                        }
                    }
                }
            }
        },
        "9/16" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "12 tpi",
            "entries" : {
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
                                "50%" : {"tapDrillDiameter" : "0.5156 in", "holeDiameter" : "0.5781 in", "cBoreDiameter" : "29/32 in", "cBoreDepth" : "0.5625 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5625 in"},
                                "75%" : {"tapDrillDiameter" : "0.4844 in", "holeDiameter" : "0.5781 in", "cBoreDiameter" : "29/32 in", "cBoreDepth" : "0.5625 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5625 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5156 in", "holeDiameter" : "0.5938 in", "cBoreDiameter" : "29/32 in", "cBoreDepth" : "0.5625 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5625 in"},
                                "75%" : {"tapDrillDiameter" : "0.4844 in", "holeDiameter" : "0.5938 in", "cBoreDiameter" : "29/32 in", "cBoreDepth" : "0.5625 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5625 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.5312 in", "holeDiameter" : "0.5781 in", "cBoreDiameter" : "29/32 in", "cBoreDepth" : "0.5625 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5625 in"},
                                "75%" : {"tapDrillDiameter" : "0.5156 in", "holeDiameter" : "0.5781 in", "cBoreDiameter" : "29/32 in", "cBoreDepth" : "0.5625 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5625 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5312 in", "holeDiameter" : "0.5938 in", "cBoreDiameter" : "29/32 in", "cBoreDepth" : "0.5625 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5625 in"},
                                "75%" : {"tapDrillDiameter" : "0.5156 in", "holeDiameter" : "0.5938 in", "cBoreDiameter" : "29/32 in", "cBoreDepth" : "0.5625 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5625 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.5312 in", "holeDiameter" : "0.5781 in", "cBoreDiameter" : "29/32 in", "cBoreDepth" : "0.5625 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5625 in"},
                                "75%" : {"tapDrillDiameter" : "0.5156 in", "holeDiameter" : "0.5781 in", "cBoreDiameter" : "29/32 in", "cBoreDepth" : "0.5625 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5625 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5312 in", "holeDiameter" : "0.5938 in", "cBoreDiameter" : "29/32 in", "cBoreDepth" : "0.5625 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5625 in"},
                                "75%" : {"tapDrillDiameter" : "0.5156 in", "holeDiameter" : "0.5938 in", "cBoreDiameter" : "29/32 in", "cBoreDepth" : "0.5625 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.5625 in"}
                            }
                        }
                    }
                }
            }
        },
        "5/8" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "11 tpi",
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
                                "50%" : {"tapDrillDiameter" : "0.5625 in", "holeDiameter" : "0.6406 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"},
                                "75%" : {"tapDrillDiameter" : "0.5312 in", "holeDiameter" : "0.6406 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5625 in", "holeDiameter" : "0.6562 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"},
                                "75%" : {"tapDrillDiameter" : "0.5312 in", "holeDiameter" : "0.6562 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5625 in", "holeDiameter" : "0.656 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"},
                                "75%" : {"tapDrillDiameter" : "0.5312 in", "holeDiameter" : "0.656 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5625 in", "holeDiameter" : "0.688 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"},
                                "75%" : {"tapDrillDiameter" : "0.5312 in", "holeDiameter" : "0.688 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5625 in", "holeDiameter" : "0.734 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"},
                                "75%" : {"tapDrillDiameter" : "0.5312 in", "holeDiameter" : "0.734 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.5938 in", "holeDiameter" : "0.6406 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"},
                                "75%" : {"tapDrillDiameter" : "0.5781 in", "holeDiameter" : "0.6406 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5938 in", "holeDiameter" : "0.6562 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"},
                                "75%" : {"tapDrillDiameter" : "0.5781 in", "holeDiameter" : "0.6562 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5938 in", "holeDiameter" : "0.656 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"},
                                "75%" : {"tapDrillDiameter" : "0.5781 in", "holeDiameter" : "0.656 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5938 in", "holeDiameter" : "0.688 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"},
                                "75%" : {"tapDrillDiameter" : "0.5781 in", "holeDiameter" : "0.688 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5938 in", "holeDiameter" : "0.734 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"},
                                "75%" : {"tapDrillDiameter" : "0.5781 in", "holeDiameter" : "0.734 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.5938 in", "holeDiameter" : "0.6406 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"},
                                "75%" : {"tapDrillDiameter" : "0.5781 in", "holeDiameter" : "0.6406 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5938 in", "holeDiameter" : "0.6562 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"},
                                "75%" : {"tapDrillDiameter" : "0.5781 in", "holeDiameter" : "0.6562 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5938 in", "holeDiameter" : "0.656 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"},
                                "75%" : {"tapDrillDiameter" : "0.5781 in", "holeDiameter" : "0.656 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5938 in", "holeDiameter" : "0.688 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"},
                                "75%" : {"tapDrillDiameter" : "0.5781 in", "holeDiameter" : "0.688 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5938 in", "holeDiameter" : "0.734 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"},
                                "75%" : {"tapDrillDiameter" : "0.5781 in", "holeDiameter" : "0.734 in", "cBoreDiameter" : "1 in", "cBoreDepth" : "0.625 in", "cSinkDiameter" : "1.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.625 in"}
                            }
                        }
                    }
                }
            }
        },
        "3/4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "10 tpi",
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
                                "50%" : {"tapDrillDiameter" : "0.6875 in", "holeDiameter" : "0.7656 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"},
                                "75%" : {"tapDrillDiameter" : "0.6562 in", "holeDiameter" : "0.7656 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.6875 in", "holeDiameter" : "0.7812 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"},
                                "75%" : {"tapDrillDiameter" : "0.6562 in", "holeDiameter" : "0.7812 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.6875 in", "holeDiameter" : "0.781 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"},
                                "75%" : {"tapDrillDiameter" : "0.6562 in", "holeDiameter" : "0.781 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.6875 in", "holeDiameter" : "0.812 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"},
                                "75%" : {"tapDrillDiameter" : "0.6562 in", "holeDiameter" : "0.812 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.6875 in", "holeDiameter" : "0.906 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"},
                                "75%" : {"tapDrillDiameter" : "0.6562 in", "holeDiameter" : "0.906 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.7031 in", "holeDiameter" : "0.7656 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"},
                                "75%" : {"tapDrillDiameter" : "0.6875 in", "holeDiameter" : "0.7656 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7031 in", "holeDiameter" : "0.7812 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"},
                                "75%" : {"tapDrillDiameter" : "0.6875 in", "holeDiameter" : "0.7812 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7031 in", "holeDiameter" : "0.781 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"},
                                "75%" : {"tapDrillDiameter" : "0.6875 in", "holeDiameter" : "0.781 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7031 in", "holeDiameter" : "0.812 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"},
                                "75%" : {"tapDrillDiameter" : "0.6875 in", "holeDiameter" : "0.812 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7031 in", "holeDiameter" : "0.906 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"},
                                "75%" : {"tapDrillDiameter" : "0.6875 in", "holeDiameter" : "0.906 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.7188 in", "holeDiameter" : "0.7656 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"},
                                "75%" : {"tapDrillDiameter" : "0.7031 in", "holeDiameter" : "0.7656 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7188 in", "holeDiameter" : "0.7812 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"},
                                "75%" : {"tapDrillDiameter" : "0.7031 in", "holeDiameter" : "0.7812 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7188 in", "holeDiameter" : "0.781 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"},
                                "75%" : {"tapDrillDiameter" : "0.7031 in", "holeDiameter" : "0.781 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7188 in", "holeDiameter" : "0.812 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"},
                                "75%" : {"tapDrillDiameter" : "0.7031 in", "holeDiameter" : "0.812 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7188 in", "holeDiameter" : "0.906 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"},
                                "75%" : {"tapDrillDiameter" : "0.7031 in", "holeDiameter" : "0.906 in", "cBoreDiameter" : "1 3/16 in", "cBoreDepth" : "0.75 in", "cSinkDiameter" : "1.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.75 in"}
                            }
                        }
                    }
                }
            }
        },
        "7/8" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "9 tpi",
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
                                "50%" : {"tapDrillDiameter" : "0.7969 in", "holeDiameter" : "0.8906 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"},
                                "75%" : {"tapDrillDiameter" : "0.7656 in", "holeDiameter" : "0.8906 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7969 in", "holeDiameter" : "0.9062 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"},
                                "75%" : {"tapDrillDiameter" : "0.7656 in", "holeDiameter" : "0.9062 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7969 in", "holeDiameter" : "0.906 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"},
                                "75%" : {"tapDrillDiameter" : "0.7656 in", "holeDiameter" : "0.906 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7969 in", "holeDiameter" : "0.938 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"},
                                "75%" : {"tapDrillDiameter" : "0.7656 in", "holeDiameter" : "0.938 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7969 in", "holeDiameter" : "1.031 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"},
                                "75%" : {"tapDrillDiameter" : "0.7656 in", "holeDiameter" : "1.031 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.8281 in", "holeDiameter" : "0.8906 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"},
                                "75%" : {"tapDrillDiameter" : "0.8125 in", "holeDiameter" : "0.8906 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.8281 in", "holeDiameter" : "0.9062 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"},
                                "75%" : {"tapDrillDiameter" : "0.8125 in", "holeDiameter" : "0.9062 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.8281 in", "holeDiameter" : "0.906 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"},
                                "75%" : {"tapDrillDiameter" : "0.8125 in", "holeDiameter" : "0.906 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.8281 in", "holeDiameter" : "0.938 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"},
                                "75%" : {"tapDrillDiameter" : "0.8125 in", "holeDiameter" : "0.938 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.8281 in", "holeDiameter" : "1.031 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"},
                                "75%" : {"tapDrillDiameter" : "0.8125 in", "holeDiameter" : "1.031 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.8438 in", "holeDiameter" : "0.8906 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"},
                                "75%" : {"tapDrillDiameter" : "0.8281 in", "holeDiameter" : "0.8906 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.8438 in", "holeDiameter" : "0.9062 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"},
                                "75%" : {"tapDrillDiameter" : "0.8281 in", "holeDiameter" : "0.9062 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.8438 in", "holeDiameter" : "0.906 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"},
                                "75%" : {"tapDrillDiameter" : "0.8281 in", "holeDiameter" : "0.906 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.8438 in", "holeDiameter" : "0.938 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"},
                                "75%" : {"tapDrillDiameter" : "0.8281 in", "holeDiameter" : "0.938 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.8438 in", "holeDiameter" : "1.031 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"},
                                "75%" : {"tapDrillDiameter" : "0.8281 in", "holeDiameter" : "1.031 in", "cBoreDiameter" : "1 3/8 in", "cBoreDepth" : "0.875 in", "cSinkDiameter" : "1.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "0.875 in"}
                            }
                        }
                    }
                }
            }
        },
        "1" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "8 tpi",
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
                                "50%" : {"tapDrillDiameter" : "0.9219 in", "holeDiameter" : "1.0156 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"},
                                "75%" : {"tapDrillDiameter" : "0.875 in", "holeDiameter" : "1.0156 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9219 in", "holeDiameter" : "1.0313 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"},
                                "75%" : {"tapDrillDiameter" : "0.875 in", "holeDiameter" : "1.0313 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9219 in", "holeDiameter" : "1.031 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"},
                                "75%" : {"tapDrillDiameter" : "0.875 in", "holeDiameter" : "1.031 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9219 in", "holeDiameter" : "1.094 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"},
                                "75%" : {"tapDrillDiameter" : "0.875 in", "holeDiameter" : "1.094 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9219 in", "holeDiameter" : "1.156 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"},
                                "75%" : {"tapDrillDiameter" : "0.875 in", "holeDiameter" : "1.156 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.9531 in", "holeDiameter" : "1.0156 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"},
                                "75%" : {"tapDrillDiameter" : "0.9219 in", "holeDiameter" : "1.0156 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9531 in", "holeDiameter" : "1.0313 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"},
                                "75%" : {"tapDrillDiameter" : "0.9219 in", "holeDiameter" : "1.0313 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9531 in", "holeDiameter" : "1.031 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"},
                                "75%" : {"tapDrillDiameter" : "0.9219 in", "holeDiameter" : "1.031 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9531 in", "holeDiameter" : "1.094 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"},
                                "75%" : {"tapDrillDiameter" : "0.9219 in", "holeDiameter" : "1.094 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9531 in", "holeDiameter" : "1.156 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"},
                                "75%" : {"tapDrillDiameter" : "0.9219 in", "holeDiameter" : "1.156 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"}
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
                                "50%" : {"tapDrillDiameter" : "0.9688 in", "holeDiameter" : "1.0156 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"},
                                "75%" : {"tapDrillDiameter" : "0.9531 in", "holeDiameter" : "1.0156 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9688 in", "holeDiameter" : "1.0313 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"},
                                "75%" : {"tapDrillDiameter" : "0.9531 in", "holeDiameter" : "1.0313 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9688 in", "holeDiameter" : "1.031 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"},
                                "75%" : {"tapDrillDiameter" : "0.9531 in", "holeDiameter" : "1.031 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9688 in", "holeDiameter" : "1.094 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"},
                                "75%" : {"tapDrillDiameter" : "0.9531 in", "holeDiameter" : "1.094 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9688 in", "holeDiameter" : "1.156 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"},
                                "75%" : {"tapDrillDiameter" : "0.9531 in", "holeDiameter" : "1.156 in", "cBoreDiameter" : "1 5/8 in", "cBoreDepth" : "1 in", "cSinkDiameter" : "1.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1 in"}
                            }
                        }
                    }
                }
            }
        },
        "1 1/8" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "7 tpi",
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
                                "50%" : {"tapDrillDiameter" : "1.0313 in", "holeDiameter" : "1.1406 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.125 in"},
                                "75%" : {"tapDrillDiameter" : "0.9844 in", "holeDiameter" : "1.1406 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.125 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.0313 in", "holeDiameter" : "1.1562 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.125 in"},
                                "75%" : {"tapDrillDiameter" : "0.9844 in", "holeDiameter" : "1.1562 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.125 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.0313 in", "holeDiameter" : "1.156 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.125 in"},
                                "75%" : {"tapDrillDiameter" : "0.9844 in", "holeDiameter" : "1.156 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.125 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.0313 in", "holeDiameter" : "1.219 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.125 in"},
                                "75%" : {"tapDrillDiameter" : "0.9844 in", "holeDiameter" : "1.219 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.125 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.0313 in", "holeDiameter" : "1.312 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.125 in"},
                                "75%" : {"tapDrillDiameter" : "0.9844 in", "holeDiameter" : "1.312 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.125 in"}
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
                                "50%" : {"tapDrillDiameter" : "1.0781 in", "holeDiameter" : "1.1406 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.125 in"},
                                "75%" : {"tapDrillDiameter" : "1.0469 in", "holeDiameter" : "1.1406 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.125 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.0781 in", "holeDiameter" : "1.1562 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.125 in"},
                                "75%" : {"tapDrillDiameter" : "1.0469 in", "holeDiameter" : "1.1562 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.125 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.0781 in", "holeDiameter" : "1.156 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.125 in"},
                                "75%" : {"tapDrillDiameter" : "1.0469 in", "holeDiameter" : "1.156 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.125 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.0781 in", "holeDiameter" : "1.219 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.125 in"},
                                "75%" : {"tapDrillDiameter" : "1.0469 in", "holeDiameter" : "1.219 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.125 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.0781 in", "holeDiameter" : "1.312 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.125 in"},
                                "75%" : {"tapDrillDiameter" : "1.0469 in", "holeDiameter" : "1.312 in", "cBoreDiameter" : "1.8125 in", "cBoreDepth" : "1.125 in", "cSinkDiameter" : "2.188 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.125 in"}
                            }
                        }
                    }
                }
            }
        },
        "1 1/4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "7 tpi",
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
                                "50%" : {"tapDrillDiameter" : "1.1562 in", "holeDiameter" : "1.2656 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"},
                                "75%" : {"tapDrillDiameter" : "1.1094 in", "holeDiameter" : "1.2656 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.1562 in", "holeDiameter" : "1.2812 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"},
                                "75%" : {"tapDrillDiameter" : "1.1094 in", "holeDiameter" : "1.2812 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.1562 in", "holeDiameter" : "1.281 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"},
                                "75%" : {"tapDrillDiameter" : "1.1094 in", "holeDiameter" : "1.281 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.1562 in", "holeDiameter" : "1.344 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"},
                                "75%" : {"tapDrillDiameter" : "1.1094 in", "holeDiameter" : "1.344 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.1562 in", "holeDiameter" : "1.438 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"},
                                "75%" : {"tapDrillDiameter" : "1.1094 in", "holeDiameter" : "1.438 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"}
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
                                "50%" : {"tapDrillDiameter" : "1.2031 in", "holeDiameter" : "1.2656 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"},
                                "75%" : {"tapDrillDiameter" : "1.1719 in", "holeDiameter" : "1.2656 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2031 in", "holeDiameter" : "1.2812 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"},
                                "75%" : {"tapDrillDiameter" : "1.1719 in", "holeDiameter" : "1.2812 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2031 in", "holeDiameter" : "1.281 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"},
                                "75%" : {"tapDrillDiameter" : "1.1719 in", "holeDiameter" : "1.281 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2031 in", "holeDiameter" : "1.344 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"},
                                "75%" : {"tapDrillDiameter" : "1.1719 in", "holeDiameter" : "1.344 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2031 in", "holeDiameter" : "1.438 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"},
                                "75%" : {"tapDrillDiameter" : "1.1719 in", "holeDiameter" : "1.438 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"}
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
                                "50%" : {"tapDrillDiameter" : "1.2187 in", "holeDiameter" : "1.2656 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"},
                                "75%" : {"tapDrillDiameter" : "1.1875 in", "holeDiameter" : "1.2656 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2187 in", "holeDiameter" : "1.2812 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"},
                                "75%" : {"tapDrillDiameter" : "1.1875 in", "holeDiameter" : "1.2812 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2187 in", "holeDiameter" : "1.281 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"},
                                "75%" : {"tapDrillDiameter" : "1.1875 in", "holeDiameter" : "1.281 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2187 in", "holeDiameter" : "1.344 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"},
                                "75%" : {"tapDrillDiameter" : "1.1875 in", "holeDiameter" : "1.344 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2187 in", "holeDiameter" : "1.438 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"},
                                "75%" : {"tapDrillDiameter" : "1.1875 in", "holeDiameter" : "1.438 in", "cBoreDiameter" : "2 in", "cBoreDepth" : "1.25 in", "cSinkDiameter" : "2.438 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.25 in"}
                            }
                        }
                    }
                }
            }
        },
        "1 3/8" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "6 tpi",
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
                                "50%" : {"tapDrillDiameter" : "1.2656 in", "holeDiameter" : "1.3906 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.375 in"},
                                "75%" : {"tapDrillDiameter" : "1.2187 in", "holeDiameter" : "1.3906 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.375 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2656 in", "holeDiameter" : "1.4062 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.375 in"},
                                "75%" : {"tapDrillDiameter" : "1.2187 in", "holeDiameter" : "1.4062 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.375 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2656 in", "holeDiameter" : "1.438 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.375 in"},
                                "75%" : {"tapDrillDiameter" : "1.2187 in", "holeDiameter" : "1.438 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.375 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2656 in", "holeDiameter" : "1.5 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.375 in"},
                                "75%" : {"tapDrillDiameter" : "1.2187 in", "holeDiameter" : "1.5 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.375 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2656 in", "holeDiameter" : "1.609 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.375 in"},
                                "75%" : {"tapDrillDiameter" : "1.2187 in", "holeDiameter" : "1.609 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.375 in"}
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
                                "50%" : {"tapDrillDiameter" : "1.3281 in", "holeDiameter" : "1.3906 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.375 in"},
                                "75%" : {"tapDrillDiameter" : "1.2969 in", "holeDiameter" : "1.3906 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.375 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.3281 in", "holeDiameter" : "1.4062 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.375 in"},
                                "75%" : {"tapDrillDiameter" : "1.2969 in", "holeDiameter" : "1.4062 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.375 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.3281 in", "holeDiameter" : "1.438 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.375 in"},
                                "75%" : {"tapDrillDiameter" : "1.2969 in", "holeDiameter" : "1.438 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.375 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.3281 in", "holeDiameter" : "1.5 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.375 in"},
                                "75%" : {"tapDrillDiameter" : "1.2969 in", "holeDiameter" : "1.5 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.375 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.3281 in", "holeDiameter" : "1.609 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.375 in"},
                                "75%" : {"tapDrillDiameter" : "1.2969 in", "holeDiameter" : "1.609 in", "cBoreDiameter" : "2.1875 in", "cBoreDepth" : "1.375 in", "cSinkDiameter" : "2.688 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.375 in"}
                            }
                        }
                    }
                }
            }
        },
        "1 1/2" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "6 tpi",
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
                                "50%" : {"tapDrillDiameter" : "1.3906 in", "holeDiameter" : "1.5156 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"},
                                "75%" : {"tapDrillDiameter" : "1.3437 in", "holeDiameter" : "1.5156 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.3906 in", "holeDiameter" : "1.5312 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"},
                                "75%" : {"tapDrillDiameter" : "1.3437 in", "holeDiameter" : "1.5312 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.3906 in", "holeDiameter" : "1.562 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"},
                                "75%" : {"tapDrillDiameter" : "1.3437 in", "holeDiameter" : "1.562 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.3906 in", "holeDiameter" : "1.625 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"},
                                "75%" : {"tapDrillDiameter" : "1.3437 in", "holeDiameter" : "1.625 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.3906 in", "holeDiameter" : "1.734 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"},
                                "75%" : {"tapDrillDiameter" : "1.3437 in", "holeDiameter" : "1.734 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"}
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
                                "50%" : {"tapDrillDiameter" : "1.4375 in", "holeDiameter" : "1.5156 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"},
                                "75%" : {"tapDrillDiameter" : "1.4219 in", "holeDiameter" : "1.5156 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.4375 in", "holeDiameter" : "1.5312 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"},
                                "75%" : {"tapDrillDiameter" : "1.4219 in", "holeDiameter" : "1.5312 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.4375 in", "holeDiameter" : "1.562 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"},
                                "75%" : {"tapDrillDiameter" : "1.4219 in", "holeDiameter" : "1.562 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.4375 in", "holeDiameter" : "1.625 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"},
                                "75%" : {"tapDrillDiameter" : "1.4219 in", "holeDiameter" : "1.625 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.4375 in", "holeDiameter" : "1.734 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"},
                                "75%" : {"tapDrillDiameter" : "1.4219 in", "holeDiameter" : "1.734 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"}
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
                                "50%" : {"tapDrillDiameter" : "1.4687 in", "holeDiameter" : "1.5156 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"},
                                "75%" : {"tapDrillDiameter" : "1.4375 in", "holeDiameter" : "1.5156 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.4687 in", "holeDiameter" : "1.5312 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"},
                                "75%" : {"tapDrillDiameter" : "1.4375 in", "holeDiameter" : "1.5312 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"}
                            }
                        },
                        "Close (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.4687 in", "holeDiameter" : "1.562 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"},
                                "75%" : {"tapDrillDiameter" : "1.4375 in", "holeDiameter" : "1.562 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"}
                            }
                        },
                        "Normal (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.4687 in", "holeDiameter" : "1.625 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"},
                                "75%" : {"tapDrillDiameter" : "1.4375 in", "holeDiameter" : "1.625 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"}
                            }
                        },
                        "Loose (ASME)" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.4687 in", "holeDiameter" : "1.734 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"},
                                "75%" : {"tapDrillDiameter" : "1.4375 in", "holeDiameter" : "1.734 in", "cBoreDiameter" : "2 3/8 in", "cBoreDepth" : "1.5 in", "cSinkDiameter" : "2.938 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.5 in"}
                            }
                        }
                    }
                }
            }
        },
        "1 3/4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "5 tpi",
            "entries" : {
                "5 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.6417 in", "holeDiameter" : "1.7656 in", "cBoreDiameter" : "2 3/4 in", "cBoreDepth" : "1.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.75 in"},
                                "75%" : {"tapDrillDiameter" : "1.5625 in", "holeDiameter" : "1.7656 in", "cBoreDiameter" : "2 3/4 in", "cBoreDepth" : "1.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.75 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.6417 in", "holeDiameter" : "1.7812 in", "cBoreDiameter" : "2 3/4 in", "cBoreDepth" : "1.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.75 in"},
                                "75%" : {"tapDrillDiameter" : "1.5625 in", "holeDiameter" : "1.7812 in", "cBoreDiameter" : "2 3/4 in", "cBoreDepth" : "1.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.75 in"}
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
                                "50%" : {"tapDrillDiameter" : "1.6959 in", "holeDiameter" : "1.7656 in", "cBoreDiameter" : "2 3/4 in", "cBoreDepth" : "1.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.75 in"},
                                "75%" : {"tapDrillDiameter" : "1.6735 in", "holeDiameter" : "1.7656 in", "cBoreDiameter" : "2 3/4 in", "cBoreDepth" : "1.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.75 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.6959 in", "holeDiameter" : "1.7812 in", "cBoreDiameter" : "2 3/4 in", "cBoreDepth" : "1.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.75 in"},
                                "75%" : {"tapDrillDiameter" : "1.6735 in", "holeDiameter" : "1.7812 in", "cBoreDiameter" : "2 3/4 in", "cBoreDepth" : "1.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.75 in"}
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
                                "50%" : {"tapDrillDiameter" : "1.7094 in", "holeDiameter" : "1.7656 in", "cBoreDiameter" : "2 3/4 in", "cBoreDepth" : "1.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.75 in"},
                                "75%" : {"tapDrillDiameter" : "1.6925 in", "holeDiameter" : "1.7656 in", "cBoreDiameter" : "2 3/4 in", "cBoreDepth" : "1.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.75 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.7094 in", "holeDiameter" : "1.7812 in", "cBoreDiameter" : "2 3/4 in", "cBoreDepth" : "1.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.75 in"},
                                "75%" : {"tapDrillDiameter" : "1.6925 in", "holeDiameter" : "1.7812 in", "cBoreDiameter" : "2 3/4 in", "cBoreDepth" : "1.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "1.75 in"}
                            }
                        }
                    }
                }
            }
        },
        "2" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "4 1/2 tpi",
            "entries" : {
                "4 1/2 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.8594 in", "holeDiameter" : "2.0156 in", "cBoreDiameter" : "3.125 in", "cBoreDepth" : "2 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2 in"},
                                "75%" : {"tapDrillDiameter" : "1.7812 in", "holeDiameter" : "2.0156 in", "cBoreDiameter" : "3.125 in", "cBoreDepth" : "2 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.8594 in", "holeDiameter" : "2.0312 in", "cBoreDiameter" : "3.125 in", "cBoreDepth" : "2 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2 in"},
                                "75%" : {"tapDrillDiameter" : "1.7812 in", "holeDiameter" : "2.0312 in", "cBoreDiameter" : "3.125 in", "cBoreDepth" : "2 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2 in"}
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
                                "50%" : {"tapDrillDiameter" : "1.9531 in", "holeDiameter" : "2.0156 in", "cBoreDiameter" : "3.125 in", "cBoreDepth" : "2 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2 in"},
                                "75%" : {"tapDrillDiameter" : "1.9219 in", "holeDiameter" : "2.0156 in", "cBoreDiameter" : "3.125 in", "cBoreDepth" : "2 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.9531 in", "holeDiameter" : "2.0312 in", "cBoreDiameter" : "3.125 in", "cBoreDepth" : "2 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2 in"},
                                "75%" : {"tapDrillDiameter" : "1.9219 in", "holeDiameter" : "2.0312 in", "cBoreDiameter" : "3.125 in", "cBoreDepth" : "2 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2 in"}
                            }
                        }
                    }
                }
            }
        },
        "2 1/4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "4 1/2 tpi",
            "entries" : {
                "4 1/2 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.1057 in", "holeDiameter" : "2.2656 in", "cBoreDiameter" : "3.5 in", "cBoreDepth" : "2.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.25 in"},
                                "75%" : {"tapDrillDiameter" : "2.036 in", "holeDiameter" : "2.2656 in", "cBoreDiameter" : "3.5 in", "cBoreDepth" : "2.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.25 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.1057 in", "holeDiameter" : "2.2812 in", "cBoreDiameter" : "3.5 in", "cBoreDepth" : "2.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.25 in"},
                                "75%" : {"tapDrillDiameter" : "2.036 in", "holeDiameter" : "2.2812 in", "cBoreDiameter" : "3.5 in", "cBoreDepth" : "2.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.25 in"}
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
                                "50%" : {"tapDrillDiameter" : "2.1959 in", "holeDiameter" : "2.2656 in", "cBoreDiameter" : "3.5 in", "cBoreDepth" : "2.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.25 in"},
                                "75%" : {"tapDrillDiameter" : "2.1735 in", "holeDiameter" : "2.2656 in", "cBoreDiameter" : "3.5 in", "cBoreDepth" : "2.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.25 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.1959 in", "holeDiameter" : "2.2812 in", "cBoreDiameter" : "3.5 in", "cBoreDepth" : "2.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.25 in"},
                                "75%" : {"tapDrillDiameter" : "2.1735 in", "holeDiameter" : "2.2812 in", "cBoreDiameter" : "3.5 in", "cBoreDepth" : "2.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.25 in"}
                            }
                        }
                    }
                }
            }
        },
        "2 1/2" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "4 tpi",
            "entries" : {
                "4 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.3376 in", "holeDiameter" : "2.5156 in", "cBoreDiameter" : "3.875 in", "cBoreDepth" : "2.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.5 in"},
                                "75%" : {"tapDrillDiameter" : "2.2575 in", "holeDiameter" : "2.5156 in", "cBoreDiameter" : "3.875 in", "cBoreDepth" : "2.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.5 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.3376 in", "holeDiameter" : "2.5312 in", "cBoreDiameter" : "3.875 in", "cBoreDepth" : "2.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.5 in"},
                                "75%" : {"tapDrillDiameter" : "2.2575 in", "holeDiameter" : "2.5312 in", "cBoreDiameter" : "3.875 in", "cBoreDepth" : "2.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.5 in"}
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
                                "50%" : {"tapDrillDiameter" : "2.4459 in", "holeDiameter" : "2.5156 in", "cBoreDiameter" : "3.875 in", "cBoreDepth" : "2.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.5 in"},
                                "75%" : {"tapDrillDiameter" : "2.435 in", "holeDiameter" : "2.5156 in", "cBoreDiameter" : "3.875 in", "cBoreDepth" : "2.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.5 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.4459 in", "holeDiameter" : "2.5312 in", "cBoreDiameter" : "3.875 in", "cBoreDepth" : "2.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.5 in"},
                                "75%" : {"tapDrillDiameter" : "2.435 in", "holeDiameter" : "2.5312 in", "cBoreDiameter" : "3.875 in", "cBoreDepth" : "2.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.5 in"}
                            }
                        }
                    }
                }
            }
        },
        "2 3/4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "4 tpi",
            "entries" : {
                "4 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.5876 in", "holeDiameter" : "2.7656 in", "cBoreDiameter" : "4.25 in", "cBoreDepth" : "2.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.75 in"},
                                "75%" : {"tapDrillDiameter" : "2.5075 in", "holeDiameter" : "2.7656 in", "cBoreDiameter" : "4.25 in", "cBoreDepth" : "2.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.75 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.5876 in", "holeDiameter" : "2.7812 in", "cBoreDiameter" : "4.25 in", "cBoreDepth" : "2.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.75 in"},
                                "75%" : {"tapDrillDiameter" : "2.5075 in", "holeDiameter" : "2.7812 in", "cBoreDiameter" : "4.25 in", "cBoreDepth" : "2.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.75 in"}
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
                                "50%" : {"tapDrillDiameter" : "2.6959 in", "holeDiameter" : "2.7656 in", "cBoreDiameter" : "4.25 in", "cBoreDepth" : "2.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.75 in"},
                                "75%" : {"tapDrillDiameter" : "2.6735 in", "holeDiameter" : "2.7656 in", "cBoreDiameter" : "4.25 in", "cBoreDepth" : "2.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.75 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.6959 in", "holeDiameter" : "2.7812 in", "cBoreDiameter" : "4.25 in", "cBoreDepth" : "2.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.75 in"},
                                "75%" : {"tapDrillDiameter" : "2.6735 in", "holeDiameter" : "2.7812 in", "cBoreDiameter" : "4.25 in", "cBoreDepth" : "2.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "2.75 in"}
                            }
                        }
                    }
                }
            }
        },
        "3" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "4 tpi",
            "entries" : {
                "4 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.8376 in", "holeDiameter" : "3.0156 in", "cBoreDiameter" : "4.625 in", "cBoreDepth" : "3 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3 in"},
                                "75%" : {"tapDrillDiameter" : "2.7575 in", "holeDiameter" : "3.0156 in", "cBoreDiameter" : "4.625 in", "cBoreDepth" : "3 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.8376 in", "holeDiameter" : "3.0312 in", "cBoreDiameter" : "4.625 in", "cBoreDepth" : "3 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3 in"},
                                "75%" : {"tapDrillDiameter" : "2.7575 in", "holeDiameter" : "3.0312 in", "cBoreDiameter" : "4.625 in", "cBoreDepth" : "3 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3 in"}
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
                                "50%" : {"tapDrillDiameter" : "2.9459 in", "holeDiameter" : "3.0156 in", "cBoreDiameter" : "4.625 in", "cBoreDepth" : "3 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3 in"},
                                "75%" : {"tapDrillDiameter" : "2.9235 in", "holeDiameter" : "3.0156 in", "cBoreDiameter" : "4.625 in", "cBoreDepth" : "3 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.9459 in", "holeDiameter" : "3.0312 in", "cBoreDiameter" : "4.625 in", "cBoreDepth" : "3 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3 in"},
                                "75%" : {"tapDrillDiameter" : "2.9235 in", "holeDiameter" : "3.0312 in", "cBoreDiameter" : "4.625 in", "cBoreDepth" : "3 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3 in"}
                            }
                        }
                    }
                }
            }
        },
        "3 1/4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "4 tpi",
            "entries" : {
                "4 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.0876 in", "holeDiameter" : "3.2656 in", "cBoreDiameter" : "5 in", "cBoreDepth" : "3.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.25 in"},
                                "75%" : {"tapDrillDiameter" : "3.0075 in", "holeDiameter" : "3.2656 in", "cBoreDiameter" : "5 in", "cBoreDepth" : "3.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.25 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.0876 in", "holeDiameter" : "3.2812 in", "cBoreDiameter" : "5 in", "cBoreDepth" : "3.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.25 in"},
                                "75%" : {"tapDrillDiameter" : "3.0075 in", "holeDiameter" : "3.2812 in", "cBoreDiameter" : "5 in", "cBoreDepth" : "3.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.25 in"}
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
                                "50%" : {"tapDrillDiameter" : "3.1959 in", "holeDiameter" : "3.2656 in", "cBoreDiameter" : "5 in", "cBoreDepth" : "3.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.25 in"},
                                "75%" : {"tapDrillDiameter" : "3.1735 in", "holeDiameter" : "3.2656 in", "cBoreDiameter" : "5 in", "cBoreDepth" : "3.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.25 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.1959 in", "holeDiameter" : "3.2812 in", "cBoreDiameter" : "5 in", "cBoreDepth" : "3.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.25 in"},
                                "75%" : {"tapDrillDiameter" : "3.1735 in", "holeDiameter" : "3.2812 in", "cBoreDiameter" : "5 in", "cBoreDepth" : "3.25 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.25 in"}
                            }
                        }
                    }
                }
            }
        },
        "3 1/2" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "4 tpi",
            "entries" : {
                "4 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.3376 in", "holeDiameter" : "3.5156 in", "cBoreDiameter" : "5.375 in", "cBoreDepth" : "3.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.5 in"},
                                "75%" : {"tapDrillDiameter" : "3.2575 in", "holeDiameter" : "3.5156 in", "cBoreDiameter" : "5.375 in", "cBoreDepth" : "3.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.5 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.3376 in", "holeDiameter" : "3.5312 in", "cBoreDiameter" : "5.375 in", "cBoreDepth" : "3.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.5 in"},
                                "75%" : {"tapDrillDiameter" : "3.2575 in", "holeDiameter" : "3.5312 in", "cBoreDiameter" : "5.375 in", "cBoreDepth" : "3.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.5 in"}
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
                                "50%" : {"tapDrillDiameter" : "3.4459 in", "holeDiameter" : "3.5156 in", "cBoreDiameter" : "5.375 in", "cBoreDepth" : "3.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.5 in"},
                                "75%" : {"tapDrillDiameter" : "3.4235 in", "holeDiameter" : "3.5156 in", "cBoreDiameter" : "5.375 in", "cBoreDepth" : "3.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.5 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.4459 in", "holeDiameter" : "3.5312 in", "cBoreDiameter" : "5.375 in", "cBoreDepth" : "3.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.5 in"},
                                "75%" : {"tapDrillDiameter" : "3.4235 in", "holeDiameter" : "3.5312 in", "cBoreDiameter" : "5.375 in", "cBoreDepth" : "3.5 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.5 in"}
                            }
                        }
                    }
                }
            }
        },
        "3 3/4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "4 tpi",
            "entries" : {
                "4 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.5876 in", "holeDiameter" : "3.7656 in", "cBoreDiameter" : "5.75 in", "cBoreDepth" : "3.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.75 in"},
                                "75%" : {"tapDrillDiameter" : "3.5075 in", "holeDiameter" : "3.7656 in", "cBoreDiameter" : "5.75 in", "cBoreDepth" : "3.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.75 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.5876 in", "holeDiameter" : "3.7812 in", "cBoreDiameter" : "5.75 in", "cBoreDepth" : "3.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.75 in"},
                                "75%" : {"tapDrillDiameter" : "3.5075 in", "holeDiameter" : "3.7812 in", "cBoreDiameter" : "5.75 in", "cBoreDepth" : "3.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.75 in"}
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
                                "50%" : {"tapDrillDiameter" : "3.6959 in", "holeDiameter" : "3.7656 in", "cBoreDiameter" : "5.75 in", "cBoreDepth" : "3.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.75 in"},
                                "75%" : {"tapDrillDiameter" : "3.6735 in", "holeDiameter" : "3.7656 in", "cBoreDiameter" : "5.75 in", "cBoreDepth" : "3.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.75 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.6959 in", "holeDiameter" : "3.7812 in", "cBoreDiameter" : "5.75 in", "cBoreDepth" : "3.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.75 in"},
                                "75%" : {"tapDrillDiameter" : "3.6735 in", "holeDiameter" : "3.7812 in", "cBoreDiameter" : "5.75 in", "cBoreDepth" : "3.75 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "3.75 in"}
                            }
                        }
                    }
                }
            }
        },
        "4" : {
            "name" : "pitch",
            "displayName" : "Threads/inch",
            "default" : "4 tpi",
            "entries" : {
                "4 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Free",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.8376 in", "holeDiameter" : "4.0156 in", "cBoreDiameter" : "6.125 in", "cBoreDepth" : "4 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "4 in"},
                                "75%" : {"tapDrillDiameter" : "3.7575 in", "holeDiameter" : "4.0156 in", "cBoreDiameter" : "6.125 in", "cBoreDepth" : "4 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "4 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.8376 in", "holeDiameter" : "4.0312 in", "cBoreDiameter" : "6.125 in", "cBoreDepth" : "4 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "4 in"},
                                "75%" : {"tapDrillDiameter" : "3.7575 in", "holeDiameter" : "4.0312 in", "cBoreDiameter" : "6.125 in", "cBoreDepth" : "4 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "4 in"}
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
                                "50%" : {"tapDrillDiameter" : "3.9459 in", "holeDiameter" : "4.0156 in", "cBoreDiameter" : "6.125 in", "cBoreDepth" : "4 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "4 in"},
                                "75%" : {"tapDrillDiameter" : "3.9235 in", "holeDiameter" : "4.0156 in", "cBoreDiameter" : "6.125 in", "cBoreDepth" : "4 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "4 in"}
                            }
                        },
                        "Free" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.9459 in", "holeDiameter" : "4.0312 in", "cBoreDiameter" : "6.125 in", "cBoreDepth" : "4 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "4 in"},
                                "75%" : {"tapDrillDiameter" : "3.9235 in", "holeDiameter" : "4.0312 in", "cBoreDiameter" : "6.125 in", "cBoreDepth" : "4 in", "cSinkDiameter" : "-1 in", "cSinkAngle" : "82 degree", "majorDiameter" : "4 in"}
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
        "0.05" : {"holeDiameter" : "0.05 mm", "tapDrillDiameter" : "0.05 mm"},
        "0.1" : {"holeDiameter" : "0.1 mm", "tapDrillDiameter" : "0.1 mm"},
        "0.2" : {"holeDiameter" : "0.2 mm", "tapDrillDiameter" : "0.2 mm"},
        "0.3" : {"holeDiameter" : "0.3 mm", "tapDrillDiameter" : "0.3 mm"},
        "0.4" : {"holeDiameter" : "0.4 mm", "tapDrillDiameter" : "0.4 mm"},
        "0.5" : {"holeDiameter" : "0.5 mm", "tapDrillDiameter" : "0.5 mm"},
        "0.6" : {"holeDiameter" : "0.6 mm", "tapDrillDiameter" : "0.6 mm"},
        "0.7" : {"holeDiameter" : "0.7 mm", "tapDrillDiameter" : "0.7 mm"},
        "0.8" : {"holeDiameter" : "0.8 mm", "tapDrillDiameter" : "0.8 mm"},
        "0.9" : {"holeDiameter" : "0.9 mm", "tapDrillDiameter" : "0.9 mm"},
        "1" : {"holeDiameter" : "1 mm", "tapDrillDiameter" : "1 mm"},
        "1.1" : {"holeDiameter" : "1.1 mm", "tapDrillDiameter" : "1.1 mm"},
        "1.2" : {"holeDiameter" : "1.2 mm", "tapDrillDiameter" : "1.2 mm"},
        "1.3" : {"holeDiameter" : "1.3 mm", "tapDrillDiameter" : "1.3 mm"},
        "1.4" : {"holeDiameter" : "1.4 mm", "tapDrillDiameter" : "1.4 mm"},
        "1.5" : {"holeDiameter" : "1.5 mm", "tapDrillDiameter" : "1.5 mm"},
        "1.6" : {"holeDiameter" : "1.6 mm", "tapDrillDiameter" : "1.6 mm"},
        "1.7" : {"holeDiameter" : "1.7 mm", "tapDrillDiameter" : "1.7 mm"},
        "1.8" : {"holeDiameter" : "1.8 mm", "tapDrillDiameter" : "1.8 mm"},
        "1.9" : {"holeDiameter" : "1.9 mm", "tapDrillDiameter" : "1.9 mm"},
        "2" : {"holeDiameter" : "2 mm", "tapDrillDiameter" : "2 mm"},
        "2.1" : {"holeDiameter" : "2.1 mm", "tapDrillDiameter" : "2.1 mm"},
        "2.2" : {"holeDiameter" : "2.2 mm", "tapDrillDiameter" : "2.2 mm"},
        "2.3" : {"holeDiameter" : "2.3 mm", "tapDrillDiameter" : "2.3 mm"},
        "2.4" : {"holeDiameter" : "2.4 mm", "tapDrillDiameter" : "2.4 mm"},
        "2.5" : {"holeDiameter" : "2.5 mm", "tapDrillDiameter" : "2.5 mm"},
        "2.6" : {"holeDiameter" : "2.6 mm", "tapDrillDiameter" : "2.6 mm"},
        "2.7" : {"holeDiameter" : "2.7 mm", "tapDrillDiameter" : "2.7 mm"},
        "2.8" : {"holeDiameter" : "2.8 mm", "tapDrillDiameter" : "2.8 mm"},
        "2.9" : {"holeDiameter" : "2.9 mm", "tapDrillDiameter" : "2.9 mm"},
        "3" : {"holeDiameter" : "3 mm", "tapDrillDiameter" : "3 mm"},
        "3.1" : {"holeDiameter" : "3.1 mm", "tapDrillDiameter" : "3.1 mm"},
        "3.2" : {"holeDiameter" : "3.2 mm", "tapDrillDiameter" : "3.2 mm"},
        "3.3" : {"holeDiameter" : "3.3 mm", "tapDrillDiameter" : "3.3 mm"},
        "3.4" : {"holeDiameter" : "3.4 mm", "tapDrillDiameter" : "3.4 mm"},
        "3.5" : {"holeDiameter" : "3.5 mm", "tapDrillDiameter" : "3.5 mm"},
        "3.6" : {"holeDiameter" : "3.6 mm", "tapDrillDiameter" : "3.6 mm"},
        "3.7" : {"holeDiameter" : "3.7 mm", "tapDrillDiameter" : "3.7 mm"},
        "3.8" : {"holeDiameter" : "3.8 mm", "tapDrillDiameter" : "3.8 mm"},
        "3.9" : {"holeDiameter" : "3.9 mm", "tapDrillDiameter" : "3.9 mm"},
        "4" : {"holeDiameter" : "4 mm", "tapDrillDiameter" : "4 mm"},
        "4.1" : {"holeDiameter" : "4.1 mm", "tapDrillDiameter" : "4.1 mm"},
        "4.2" : {"holeDiameter" : "4.2 mm", "tapDrillDiameter" : "4.2 mm"},
        "4.3" : {"holeDiameter" : "4.3 mm", "tapDrillDiameter" : "4.3 mm"},
        "4.4" : {"holeDiameter" : "4.4 mm", "tapDrillDiameter" : "4.4 mm"},
        "4.5" : {"holeDiameter" : "4.5 mm", "tapDrillDiameter" : "4.5 mm"},
        "4.6" : {"holeDiameter" : "4.6 mm", "tapDrillDiameter" : "4.6 mm"},
        "4.7" : {"holeDiameter" : "4.7 mm", "tapDrillDiameter" : "4.7 mm"},
        "4.8" : {"holeDiameter" : "4.8 mm", "tapDrillDiameter" : "4.8 mm"},
        "4.9" : {"holeDiameter" : "4.9 mm", "tapDrillDiameter" : "4.9 mm"},
        "5" : {"holeDiameter" : "5 mm", "tapDrillDiameter" : "5 mm"},
        "5.1" : {"holeDiameter" : "5.1 mm", "tapDrillDiameter" : "5.1 mm"},
        "5.2" : {"holeDiameter" : "5.2 mm", "tapDrillDiameter" : "5.2 mm"},
        "5.3" : {"holeDiameter" : "5.3 mm", "tapDrillDiameter" : "5.3 mm"},
        "5.4" : {"holeDiameter" : "5.4 mm", "tapDrillDiameter" : "5.4 mm"},
        "5.5" : {"holeDiameter" : "5.5 mm", "tapDrillDiameter" : "5.5 mm"},
        "5.6" : {"holeDiameter" : "5.6 mm", "tapDrillDiameter" : "5.6 mm"},
        "5.7" : {"holeDiameter" : "5.7 mm", "tapDrillDiameter" : "5.7 mm"},
        "5.8" : {"holeDiameter" : "5.8 mm", "tapDrillDiameter" : "5.8 mm"},
        "5.9" : {"holeDiameter" : "5.9 mm", "tapDrillDiameter" : "5.9 mm"},
        "6" : {"holeDiameter" : "6 mm", "tapDrillDiameter" : "6 mm"},
        "6.1" : {"holeDiameter" : "6.1 mm", "tapDrillDiameter" : "6.1 mm"},
        "6.2" : {"holeDiameter" : "6.2 mm", "tapDrillDiameter" : "6.2 mm"},
        "6.3" : {"holeDiameter" : "6.3 mm", "tapDrillDiameter" : "6.3 mm"},
        "6.4" : {"holeDiameter" : "6.4 mm", "tapDrillDiameter" : "6.4 mm"},
        "6.5" : {"holeDiameter" : "6.5 mm", "tapDrillDiameter" : "6.5 mm"},
        "6.6" : {"holeDiameter" : "6.6 mm", "tapDrillDiameter" : "6.6 mm"},
        "6.7" : {"holeDiameter" : "6.7 mm", "tapDrillDiameter" : "6.7 mm"},
        "6.8" : {"holeDiameter" : "6.8 mm", "tapDrillDiameter" : "6.8 mm"},
        "6.9" : {"holeDiameter" : "6.9 mm", "tapDrillDiameter" : "6.9 mm"},
        "7" : {"holeDiameter" : "7 mm", "tapDrillDiameter" : "7 mm"},
        "7.1" : {"holeDiameter" : "7.1 mm", "tapDrillDiameter" : "7.1 mm"},
        "7.2" : {"holeDiameter" : "7.2 mm", "tapDrillDiameter" : "7.2 mm"},
        "7.3" : {"holeDiameter" : "7.3 mm", "tapDrillDiameter" : "7.3 mm"},
        "7.4" : {"holeDiameter" : "7.4 mm", "tapDrillDiameter" : "7.4 mm"},
        "7.5" : {"holeDiameter" : "7.5 mm", "tapDrillDiameter" : "7.5 mm"},
        "7.6" : {"holeDiameter" : "7.6 mm", "tapDrillDiameter" : "7.6 mm"},
        "7.7" : {"holeDiameter" : "7.7 mm", "tapDrillDiameter" : "7.7 mm"},
        "7.8" : {"holeDiameter" : "7.8 mm", "tapDrillDiameter" : "7.8 mm"},
        "7.9" : {"holeDiameter" : "7.9 mm", "tapDrillDiameter" : "7.9 mm"},
        "8" : {"holeDiameter" : "8 mm", "tapDrillDiameter" : "8 mm"},
        "8.1" : {"holeDiameter" : "8.1 mm", "tapDrillDiameter" : "8.1 mm"},
        "8.2" : {"holeDiameter" : "8.2 mm", "tapDrillDiameter" : "8.2 mm"},
        "8.3" : {"holeDiameter" : "8.3 mm", "tapDrillDiameter" : "8.3 mm"},
        "8.4" : {"holeDiameter" : "8.4 mm", "tapDrillDiameter" : "8.4 mm"},
        "8.5" : {"holeDiameter" : "8.5 mm", "tapDrillDiameter" : "8.5 mm"},
        "8.6" : {"holeDiameter" : "8.6 mm", "tapDrillDiameter" : "8.6 mm"},
        "8.7" : {"holeDiameter" : "8.7 mm", "tapDrillDiameter" : "8.7 mm"},
        "8.8" : {"holeDiameter" : "8.8 mm", "tapDrillDiameter" : "8.8 mm"},
        "8.9" : {"holeDiameter" : "8.9 mm", "tapDrillDiameter" : "8.9 mm"},
        "9" : {"holeDiameter" : "9 mm", "tapDrillDiameter" : "9 mm"},
        "9.1" : {"holeDiameter" : "9.1 mm", "tapDrillDiameter" : "9.1 mm"},
        "9.2" : {"holeDiameter" : "9.2 mm", "tapDrillDiameter" : "9.2 mm"},
        "9.3" : {"holeDiameter" : "9.3 mm", "tapDrillDiameter" : "9.3 mm"},
        "9.4" : {"holeDiameter" : "9.4 mm", "tapDrillDiameter" : "9.4 mm"},
        "9.5" : {"holeDiameter" : "9.5 mm", "tapDrillDiameter" : "9.5 mm"},
        "9.6" : {"holeDiameter" : "9.6 mm", "tapDrillDiameter" : "9.6 mm"},
        "9.7" : {"holeDiameter" : "9.7 mm", "tapDrillDiameter" : "9.7 mm"},
        "9.8" : {"holeDiameter" : "9.8 mm", "tapDrillDiameter" : "9.8 mm"},
        "9.9" : {"holeDiameter" : "9.9 mm", "tapDrillDiameter" : "9.9 mm"},
        "10" : {"holeDiameter" : "10 mm", "tapDrillDiameter" : "10 mm"},
        "10.5" : {"holeDiameter" : "10.5 mm", "tapDrillDiameter" : "10.5 mm"},
        "11" : {"holeDiameter" : "11 mm", "tapDrillDiameter" : "11 mm"},
        "11.5" : {"holeDiameter" : "11.5 mm", "tapDrillDiameter" : "11.5 mm"},
        "12" : {"holeDiameter" : "12 mm", "tapDrillDiameter" : "12 mm"},
        "12.5" : {"holeDiameter" : "12.5 mm", "tapDrillDiameter" : "12.5 mm"},
        "13" : {"holeDiameter" : "13 mm", "tapDrillDiameter" : "13 mm"},
        "13.5" : {"holeDiameter" : "13.5 mm", "tapDrillDiameter" : "13.5 mm"},
        "14" : {"holeDiameter" : "14 mm", "tapDrillDiameter" : "14 mm"},
        "14.5" : {"holeDiameter" : "14.5 mm", "tapDrillDiameter" : "14.5 mm"},
        "15" : {"holeDiameter" : "15 mm", "tapDrillDiameter" : "15 mm"},
        "15.5" : {"holeDiameter" : "15.5 mm", "tapDrillDiameter" : "15.5 mm"},
        "16" : {"holeDiameter" : "16 mm", "tapDrillDiameter" : "16 mm"},
        "16.5" : {"holeDiameter" : "16.5 mm", "tapDrillDiameter" : "16.5 mm"},
        "17" : {"holeDiameter" : "17 mm", "tapDrillDiameter" : "17 mm"},
        "17.5" : {"holeDiameter" : "17.5 mm", "tapDrillDiameter" : "17.5 mm"},
        "18" : {"holeDiameter" : "18 mm", "tapDrillDiameter" : "18 mm"},
        "18.5" : {"holeDiameter" : "18.5 mm", "tapDrillDiameter" : "18.5 mm"},
        "19" : {"holeDiameter" : "19 mm", "tapDrillDiameter" : "19 mm"},
        "19.5" : {"holeDiameter" : "19.5 mm", "tapDrillDiameter" : "19.5 mm"},
        "20" : {"holeDiameter" : "20 mm", "tapDrillDiameter" : "20 mm"},
        "20.5" : {"holeDiameter" : "20.5 mm", "tapDrillDiameter" : "20.5 mm"},
        "21" : {"holeDiameter" : "21 mm", "tapDrillDiameter" : "21 mm"},
        "21.5" : {"holeDiameter" : "21.5 mm", "tapDrillDiameter" : "21.5 mm"},
        "22" : {"holeDiameter" : "22 mm", "tapDrillDiameter" : "22 mm"},
        "22.5" : {"holeDiameter" : "22.5 mm", "tapDrillDiameter" : "22.5 mm"},
        "23" : {"holeDiameter" : "23 mm", "tapDrillDiameter" : "23 mm"},
        "23.5" : {"holeDiameter" : "23.5 mm", "tapDrillDiameter" : "23.5 mm"},
        "24" : {"holeDiameter" : "24 mm", "tapDrillDiameter" : "24 mm"},
        "24.5" : {"holeDiameter" : "24.5 mm", "tapDrillDiameter" : "24.5 mm"},
        "25" : {"holeDiameter" : "25 mm", "tapDrillDiameter" : "25 mm"},
        "25.5" : {"holeDiameter" : "25.5 mm", "tapDrillDiameter" : "25.5 mm"},
        "26" : {"holeDiameter" : "26 mm", "tapDrillDiameter" : "26 mm"},
        "26.5" : {"holeDiameter" : "26.5 mm", "tapDrillDiameter" : "26.5 mm"},
        "27" : {"holeDiameter" : "27 mm", "tapDrillDiameter" : "27 mm"},
        "27.5" : {"holeDiameter" : "27.5 mm", "tapDrillDiameter" : "27.5 mm"},
        "28" : {"holeDiameter" : "28 mm", "tapDrillDiameter" : "28 mm"},
        "28.5" : {"holeDiameter" : "28.5 mm", "tapDrillDiameter" : "28.5 mm"},
        "29" : {"holeDiameter" : "29 mm", "tapDrillDiameter" : "29 mm"},
        "29.5" : {"holeDiameter" : "29.5 mm", "tapDrillDiameter" : "29.5 mm"},
        "30" : {"holeDiameter" : "30 mm", "tapDrillDiameter" : "30 mm"},
        "30.5" : {"holeDiameter" : "30.5 mm", "tapDrillDiameter" : "30.5 mm"},
        "31" : {"holeDiameter" : "31 mm", "tapDrillDiameter" : "31 mm"},
        "31.5" : {"holeDiameter" : "31.5 mm", "tapDrillDiameter" : "31.5 mm"},
        "32" : {"holeDiameter" : "32 mm", "tapDrillDiameter" : "32 mm"},
        "32.5" : {"holeDiameter" : "32.5 mm", "tapDrillDiameter" : "32.5 mm"},
        "33" : {"holeDiameter" : "33 mm", "tapDrillDiameter" : "33 mm"},
        "33.5" : {"holeDiameter" : "33.5 mm", "tapDrillDiameter" : "33.5 mm"},
        "34" : {"holeDiameter" : "34 mm", "tapDrillDiameter" : "34 mm"},
        "34.5" : {"holeDiameter" : "34.5 mm", "tapDrillDiameter" : "34.5 mm"},
        "35" : {"holeDiameter" : "35 mm", "tapDrillDiameter" : "35 mm"},
        "35.5" : {"holeDiameter" : "35.5 mm", "tapDrillDiameter" : "35.5 mm"},
        "36" : {"holeDiameter" : "36 mm", "tapDrillDiameter" : "36 mm"},
        "36.5" : {"holeDiameter" : "36.5 mm", "tapDrillDiameter" : "36.5 mm"},
        "37" : {"holeDiameter" : "37 mm", "tapDrillDiameter" : "37 mm"},
        "37.5" : {"holeDiameter" : "37.5 mm", "tapDrillDiameter" : "37.5 mm"},
        "38" : {"holeDiameter" : "38 mm", "tapDrillDiameter" : "38 mm"}
    }
};

const ISO_ClearanceHoleTable = {
    "name" : "size",
    "displayName" : "Size",
    "default" : "M5",
    "entries" : {
        "M1" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.1 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.2 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.3 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M1.1" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.2 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.3 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.4 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M1.2" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.3 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.4 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.5 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M1.4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.5 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.6 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.8 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M1.6" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "3.5 mm", "cBoreDepth" : "1.6 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.7 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "3.5 mm", "cBoreDepth" : "1.6 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.8 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "3.5 mm", "cBoreDepth" : "1.6 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.0 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M1.8" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.0 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.1 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.2 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M2" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "4.4 mm", "cBoreDepth" : "2 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.2 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "4.4 mm", "cBoreDepth" : "2 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.4 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "4.4 mm", "cBoreDepth" : "2 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.6 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M2.2" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.4 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.6 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.8 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M2.5" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "5.5 mm", "cBoreDepth" : "2.5 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.7 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "5.5 mm", "cBoreDepth" : "2.5 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.9 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "5.5 mm", "cBoreDepth" : "2.5 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "3.1 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M3" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "6.5 mm", "cBoreDepth" : "3 mm", "cSinkDiameter" : "6.72 mm", "holeDiameter" : "3.2 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "6.5 mm", "cBoreDepth" : "3 mm", "cSinkDiameter" : "6.72 mm", "holeDiameter" : "3.4 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "6.5 mm", "cBoreDepth" : "3 mm", "cSinkDiameter" : "6.72 mm", "holeDiameter" : "3.6 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "8.25 mm", "cBoreDepth" : "4 mm", "cSinkDiameter" : "8.96 mm", "holeDiameter" : "4.3 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "8.25 mm", "cBoreDepth" : "4 mm", "cSinkDiameter" : "8.96 mm", "holeDiameter" : "4.5 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "8.25 mm", "cBoreDepth" : "4 mm", "cSinkDiameter" : "8.96 mm", "holeDiameter" : "4.8 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M4.5" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "4.8 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "5.0 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "5.3 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M5" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "9.75 mm", "cBoreDepth" : "5 mm", "cSinkDiameter" : "11.2 mm", "holeDiameter" : "5.3 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "9.75 mm", "cBoreDepth" : "5 mm", "cSinkDiameter" : "11.2 mm", "holeDiameter" : "5.5 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "9.75 mm", "cBoreDepth" : "5 mm", "cSinkDiameter" : "11.2 mm", "holeDiameter" : "5.8 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M6" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "11.25 mm", "cBoreDepth" : "6 mm", "cSinkDiameter" : "13.44 mm", "holeDiameter" : "6.4 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "11.25 mm", "cBoreDepth" : "6 mm", "cSinkDiameter" : "13.44 mm", "holeDiameter" : "6.6 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "11.25 mm", "cBoreDepth" : "6 mm", "cSinkDiameter" : "13.44 mm", "holeDiameter" : "7 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M7" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "7.4 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "7.6 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "8 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M8" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "holeDiameter" : "8.4 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "holeDiameter" : "9 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "holeDiameter" : "10 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M10" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "holeDiameter" : "10.5 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "holeDiameter" : "11 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "holeDiameter" : "12 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M12" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "holeDiameter" : "13 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "holeDiameter" : "13.5 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "holeDiameter" : "14.5 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M14" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "holeDiameter" : "15 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "holeDiameter" : "15.5 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "holeDiameter" : "16.5 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M16" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "25.5 mm", "cBoreDepth" : "16 mm", "cSinkDiameter" : "33.6 mm", "holeDiameter" : "17 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "25.5 mm", "cBoreDepth" : "16 mm", "cSinkDiameter" : "33.6 mm", "holeDiameter" : "17.5 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "25.5 mm", "cBoreDepth" : "16 mm", "cSinkDiameter" : "33.6 mm", "holeDiameter" : "18.5 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M18" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "19 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "20 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "21 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M20" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "holeDiameter" : "21 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "holeDiameter" : "22 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "holeDiameter" : "24 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M22" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "23 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "24 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "26 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M24" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "40 mm", "cBoreDepth" : "24.8 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "25 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "40 mm", "cBoreDepth" : "24.8 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "26 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "40 mm", "cBoreDepth" : "24.8 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "28 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M27" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "28 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "30 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "32 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M30" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "50 mm", "cBoreDepth" : "31 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "31 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "50 mm", "cBoreDepth" : "31 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "33 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "50 mm", "cBoreDepth" : "31 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "35 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M33" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "34 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "36 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "38 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M36" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "58 mm", "cBoreDepth" : "37 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "37 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "58 mm", "cBoreDepth" : "37 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "39 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "58 mm", "cBoreDepth" : "37 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "42 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M39" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "40 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "42 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "45 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M42" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "69.3 mm", "cBoreDepth" : "43 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "43 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "69.3 mm", "cBoreDepth" : "43 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "45 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "69.3 mm", "cBoreDepth" : "43 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "48 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M45" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "46 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "48 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "52 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M48" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "79.2 mm", "cBoreDepth" : "49 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "50 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "79.2 mm", "cBoreDepth" : "49 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "52 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "79.2 mm", "cBoreDepth" : "49 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "56 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M52" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "54 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "56 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "62 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M56" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "92.4 mm", "cBoreDepth" : "57 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "58 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "92.4 mm", "cBoreDepth" : "57 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "62 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "92.4 mm", "cBoreDepth" : "57 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "66 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M60" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "62 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "66 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "70 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M64" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "105.6 mm", "cBoreDepth" : "65 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "66 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "105.6 mm", "cBoreDepth" : "65 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "70 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "105.6 mm", "cBoreDepth" : "65 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "74 mm", "cSinkAngle" : "90 degree"}
            }
        },
        "M68" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "70 mm", "cSinkAngle" : "90 degree"},
                "Normal" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "74 mm", "cSinkAngle" : "90 degree"},
                "Loose" : {"cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "78 mm", "cSinkAngle" : "90 degree"}
            }
        }
    }
};

const ISO_TappedHoleTable = {
    "name" : "size",
    "displayName" : "Size",
    "default" : "M5",
    "entries" : {
        "M1" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.20 mm",
            "entries" : {
                "0.20 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.9 mm", "tapDrillDiameter" : "0.9 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1 mm"},
                        "75%" : {"holeDiameter" : "0.8 mm", "tapDrillDiameter" : "0.8 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1 mm"}
                    }
                },
                "0.25 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.85 mm", "tapDrillDiameter" : "0.85 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1 mm"},
                        "75%" : {"holeDiameter" : "0.75 mm", "tapDrillDiameter" : "0.75 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1 mm"}
                    }
                }
            }
        },
        "M1.1" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.20 mm",
            "entries" : {
                "0.20 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.0 mm", "tapDrillDiameter" : "1.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.1 mm"},
                        "75%" : {"holeDiameter" : "0.9 mm", "tapDrillDiameter" : "0.9 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.1 mm"}
                    }
                },
                "0.25 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.95 mm", "tapDrillDiameter" : "0.95 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.1 mm"},
                        "75%" : {"holeDiameter" : "0.85 mm", "tapDrillDiameter" : "0.85 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.1 mm"}
                    }
                }
            }
        },
        "M1.2" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.20 mm",
            "entries" : {
                "0.20 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.1 mm", "tapDrillDiameter" : "1.1 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.2 mm"},
                        "75%" : {"holeDiameter" : "1.0 mm", "tapDrillDiameter" : "1.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.2 mm"}
                    }
                },
                "0.25 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.0 mm", "tapDrillDiameter" : "1.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.2 mm"},
                        "75%" : {"holeDiameter" : "0.95 mm", "tapDrillDiameter" : "0.95 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.2 mm"}
                    }
                }
            }
        },
        "M1.4" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.20 mm",
            "entries" : {
                "0.20 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.3 mm", "tapDrillDiameter" : "1.3 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.4 mm"},
                        "75%" : {"holeDiameter" : "1.2 mm", "tapDrillDiameter" : "1.2 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.4 mm"}
                    }
                },
                "0.30 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.2 mm", "tapDrillDiameter" : "1.2 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.4 mm"},
                        "75%" : {"holeDiameter" : "1.1 mm", "tapDrillDiameter" : "1.1 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.4 mm"}
                    }
                }
            }
        },
        "M1.6" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.20 mm",
            "entries" : {
                "0.20 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.5 mm", "tapDrillDiameter" : "1.5 mm", "cBoreDiameter" : "3.5 mm", "cBoreDepth" : "1.6 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.6 mm"},
                        "75%" : {"holeDiameter" : "1.4 mm", "tapDrillDiameter" : "1.4 mm", "cBoreDiameter" : "3.5 mm", "cBoreDepth" : "1.6 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.6 mm"}
                    }
                },
                "0.35 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.35 mm", "tapDrillDiameter" : "1.35 mm", "cBoreDiameter" : "3.5 mm", "cBoreDepth" : "1.6 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.6 mm"},
                        "75%" : {"holeDiameter" : "1.25 mm", "tapDrillDiameter" : "1.25 mm", "cBoreDiameter" : "3.5 mm", "cBoreDepth" : "1.6 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.6 mm"}
                    }
                }
            }
        },
        "M1.8" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.20 mm",
            "entries" : {
                "0.20 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.7 mm", "tapDrillDiameter" : "1.7 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.8 mm"},
                        "75%" : {"holeDiameter" : "1.6 mm", "tapDrillDiameter" : "1.6 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.8 mm"}
                    }
                },
                "0.35 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.55 mm", "tapDrillDiameter" : "1.55 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.8 mm"},
                        "75%" : {"holeDiameter" : "1.45 mm", "tapDrillDiameter" : "1.45 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.8 mm"}
                    }
                }
            }
        },
        "M2" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.25 mm",
            "entries" : {
                "0.25 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.85 mm", "tapDrillDiameter" : "1.85 mm", "cBoreDiameter" : "4.4 mm", "cBoreDepth" : "2 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2 mm"},
                        "75%" : {"holeDiameter" : "1.75 mm", "tapDrillDiameter" : "1.75 mm", "cBoreDiameter" : "4.4 mm", "cBoreDepth" : "2 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2 mm"}
                    }
                },
                "0.40 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.75 mm", "tapDrillDiameter" : "1.75 mm", "cBoreDiameter" : "4.4 mm", "cBoreDepth" : "2 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2 mm"},
                        "75%" : {"holeDiameter" : "1.6 mm", "tapDrillDiameter" : "1.6 mm", "cBoreDiameter" : "4.4 mm", "cBoreDepth" : "2 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2 mm"}
                    }
                }
            }
        },
        "M2.2" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.25 mm",
            "entries" : {
                "0.25 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "2.0 mm", "tapDrillDiameter" : "2.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.2 mm"},
                        "75%" : {"holeDiameter" : "1.95 mm", "tapDrillDiameter" : "1.95 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.2 mm"}
                    }
                },
                "0.45 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.9 mm", "tapDrillDiameter" : "1.9 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.2 mm"},
                        "75%" : {"holeDiameter" : "1.75 mm", "tapDrillDiameter" : "1.75 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.2 mm"}
                    }
                }
            }
        },
        "M2.5" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.35 mm",
            "entries" : {
                "0.35 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "2.3 mm", "tapDrillDiameter" : "2.3 mm", "cBoreDiameter" : "5.5 mm", "cBoreDepth" : "2.5 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.5 mm"},
                        "75%" : {"holeDiameter" : "2.15 mm", "tapDrillDiameter" : "2.15 mm", "cBoreDiameter" : "5.5 mm", "cBoreDepth" : "2.5 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.5 mm"}
                    }
                },
                "0.45 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "2.2 mm", "tapDrillDiameter" : "2.2 mm", "cBoreDiameter" : "5.5 mm", "cBoreDepth" : "2.5 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.5 mm"},
                        "75%" : {"holeDiameter" : "2.05 mm", "tapDrillDiameter" : "2.05 mm", "cBoreDiameter" : "5.5 mm", "cBoreDepth" : "2.5 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.5 mm"}
                    }
                }
            }
        },
        "M3" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.50 mm",
            "entries" : {
                "0.35 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "2.8 mm", "tapDrillDiameter" : "2.8 mm", "cBoreDiameter" : "6.5 mm", "cBoreDepth" : "3 mm", "cSinkDiameter" : "6.72 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "3 mm"},
                        "75%" : {"holeDiameter" : "2.65 mm", "tapDrillDiameter" : "2.65 mm", "cBoreDiameter" : "6.5 mm", "cBoreDepth" : "3 mm", "cSinkDiameter" : "6.72 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "3 mm"}
                    }
                },
                "0.50 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "2.7 mm", "tapDrillDiameter" : "2.7 mm", "cBoreDiameter" : "6.5 mm", "cBoreDepth" : "3 mm", "cSinkDiameter" : "6.72 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "3 mm"},
                        "75%" : {"holeDiameter" : "2.5 mm", "tapDrillDiameter" : "2.5 mm", "cBoreDiameter" : "6.5 mm", "cBoreDepth" : "3 mm", "cSinkDiameter" : "6.72 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "3 mm"}
                    }
                }
            }
        },
        "M4" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.70 mm",
            "entries" : {
                "0.50 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "3.7 mm", "tapDrillDiameter" : "3.7 mm", "cBoreDiameter" : "8.25 mm", "cBoreDepth" : "4 mm", "cSinkDiameter" : "8.96 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4 mm"},
                        "75%" : {"holeDiameter" : "3.5 mm", "tapDrillDiameter" : "3.5 mm", "cBoreDiameter" : "8.25 mm", "cBoreDepth" : "4 mm", "cSinkDiameter" : "8.96 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4 mm"}
                    }
                },
                "0.70 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "3.5 mm", "tapDrillDiameter" : "3.5 mm", "cBoreDiameter" : "8.25 mm", "cBoreDepth" : "4 mm", "cSinkDiameter" : "8.96 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4 mm"},
                        "75%" : {"holeDiameter" : "3.3 mm", "tapDrillDiameter" : "3.3 mm", "cBoreDiameter" : "8.25 mm", "cBoreDepth" : "4 mm", "cSinkDiameter" : "8.96 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4 mm"}
                    }
                }
            }
        },
        "M4.5" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.50 mm",
            "entries" : {
                "0.50 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "4.2 mm", "tapDrillDiameter" : "4.2 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4.5 mm"},
                        "75%" : {"holeDiameter" : "4.0 mm", "tapDrillDiameter" : "4.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4.5 mm"}
                    }
                },
                "0.75 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "4.0 mm", "tapDrillDiameter" : "4.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4.5 mm"},
                        "75%" : {"holeDiameter" : "3.7 mm", "tapDrillDiameter" : "3.7 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4.5 mm"}
                    }
                }
            }
        },
        "M5" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.80 mm",
            "entries" : {
                "0.5 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "4.7 mm", "tapDrillDiameter" : "4.7 mm", "cBoreDiameter" : "9.75 mm", "cBoreDepth" : "5 mm", "cSinkDiameter" : "11.2 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "5 mm"},
                        "75%" : {"holeDiameter" : "4.5 mm", "tapDrillDiameter" : "4.5 mm", "cBoreDiameter" : "9.75 mm", "cBoreDepth" : "5 mm", "cSinkDiameter" : "11.2 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "5 mm"}
                    }
                },
                "0.80 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "4.5 mm", "tapDrillDiameter" : "4.5 mm", "cBoreDiameter" : "9.75 mm", "cBoreDepth" : "5 mm", "cSinkDiameter" : "11.2 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "5 mm"},
                        "75%" : {"holeDiameter" : "4.2 mm", "tapDrillDiameter" : "4.2 mm", "cBoreDiameter" : "9.75 mm", "cBoreDepth" : "5 mm", "cSinkDiameter" : "11.2 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "5 mm"}
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
                        "50%" : {"holeDiameter" : "5.5 mm", "tapDrillDiameter" : "5.5 mm", "cBoreDiameter" : "11.25 mm", "cBoreDepth" : "6 mm", "cSinkDiameter" : "13.44 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "6 mm"},
                        "75%" : {"holeDiameter" : "5.25 mm", "tapDrillDiameter" : "5.25 mm", "cBoreDiameter" : "11.25 mm", "cBoreDepth" : "6 mm", "cSinkDiameter" : "13.44 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "6 mm"}
                    }
                },
                "1.00 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "5.4 mm", "tapDrillDiameter" : "5.4 mm", "cBoreDiameter" : "11.25 mm", "cBoreDepth" : "6 mm", "cSinkDiameter" : "13.44 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "6 mm"},
                        "75%" : {"holeDiameter" : "5 mm", "tapDrillDiameter" : "5 mm", "cBoreDiameter" : "11.25 mm", "cBoreDepth" : "6 mm", "cSinkDiameter" : "13.44 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "6 mm"}
                    }
                }
            }
        },
        "M7" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.75 mm",
            "entries" : {
                "0.75 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "6.5 mm", "tapDrillDiameter" : "6.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "7 mm"},
                        "75%" : {"holeDiameter" : "6.2 mm", "tapDrillDiameter" : "6.2 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "7 mm"}
                    }
                },
                "1.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "6.4 mm", "tapDrillDiameter" : "6.4 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "7 mm"},
                        "75%" : {"holeDiameter" : "6.0 mm", "tapDrillDiameter" : "6.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "7 mm"}
                    }
                }
            }
        },
        "M8" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "1.25 mm",
            "entries" : {
                "0.75 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "7.5 mm", "tapDrillDiameter" : "7.5 mm", "cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "8 mm"},
                        "75%" : {"holeDiameter" : "7.2 mm", "tapDrillDiameter" : "7.2 mm", "cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "8 mm"}
                    }
                },
                "1.00 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "7.4 mm", "tapDrillDiameter" : "7.4 mm", "cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "8 mm"},
                        "75%" : {"holeDiameter" : "7 mm", "tapDrillDiameter" : "7 mm", "cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "8 mm"}
                    }
                },
                "1.25 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "7.2 mm", "tapDrillDiameter" : "7.2 mm", "cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "8 mm"},
                        "75%" : {"holeDiameter" : "6.8 mm", "tapDrillDiameter" : "6.8 mm", "cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "8 mm"}
                    }
                }
            }
        },
        "M10" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "1.50 mm",
            "entries" : {
                "1.00 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "9.4 mm", "tapDrillDiameter" : "9.4 mm", "cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "10 mm"},
                        "75%" : {"holeDiameter" : "9 mm", "tapDrillDiameter" : "9 mm", "cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "10 mm"}
                    }
                },
                "1.25 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "9.2 mm", "tapDrillDiameter" : "9.2 mm", "cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "10 mm"},
                        "75%" : {"holeDiameter" : "8.8 mm", "tapDrillDiameter" : "8.8 mm", "cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "10 mm"}
                    }
                },
                "1.50 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "9 mm", "tapDrillDiameter" : "9 mm", "cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "10 mm"},
                        "75%" : {"holeDiameter" : "8.5 mm", "tapDrillDiameter" : "8.5 mm", "cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "10 mm"}
                    }
                }
            }
        },
        "M12" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "1.75 mm",
            "entries" : {
                "1.25 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "11.2 mm", "tapDrillDiameter" : "11.2 mm", "cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "12 mm"},
                        "75%" : {"holeDiameter" : "10.8 mm", "tapDrillDiameter" : "10.8 mm", "cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "12 mm"}
                    }
                },
                "1.50 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "11 mm", "tapDrillDiameter" : "11 mm", "cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "12 mm"},
                        "75%" : {"holeDiameter" : "10.5 mm", "tapDrillDiameter" : "10.5 mm", "cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "12 mm"}
                    }
                },
                "1.75 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "11.2 mm", "tapDrillDiameter" : "11.2 mm", "cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "12 mm"},
                        "75%" : {"holeDiameter" : "10.3 mm", "tapDrillDiameter" : "10.3 mm", "cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "12 mm"}
                    }
                }
            }
        },
        "M14" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "2.00 mm",
            "entries" : {
                "1.25 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "13.2 mm", "tapDrillDiameter" : "13.2 mm", "cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "14 mm"},
                        "75%" : {"holeDiameter" : "12.8 mm", "tapDrillDiameter" : "12.8 mm", "cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "14 mm"}
                    }
                },
                "1.50 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "13 mm", "tapDrillDiameter" : "13 mm", "cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "14 mm"},
                        "75%" : {"holeDiameter" : "12.5 mm", "tapDrillDiameter" : "12.5 mm", "cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "14 mm"}
                    }
                },
                "2.00 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "12.7 mm", "tapDrillDiameter" : "12.7 mm", "cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "14 mm"},
                        "75%" : {"holeDiameter" : "12.1 mm", "tapDrillDiameter" : "12.1 mm", "cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "14 mm"}
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
                        "50%" : {"holeDiameter" : "15 mm", "tapDrillDiameter" : "15 mm", "cBoreDiameter" : "25.5 mm", "cBoreDepth" : "16 mm", "cSinkDiameter" : "33.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "16 mm"},
                        "75%" : {"holeDiameter" : "14.5 mm", "tapDrillDiameter" : "14.5 mm", "cBoreDiameter" : "25.5 mm", "cBoreDepth" : "16 mm", "cSinkDiameter" : "33.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "16 mm"}
                    }
                },
                "2.00 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "14.75 mm", "tapDrillDiameter" : "14.75 mm", "cBoreDiameter" : "25.5 mm", "cBoreDepth" : "16 mm", "cSinkDiameter" : "33.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "16 mm"},
                        "75%" : {"holeDiameter" : "14 mm", "tapDrillDiameter" : "14 mm", "cBoreDiameter" : "25.5 mm", "cBoreDepth" : "16 mm", "cSinkDiameter" : "33.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "16 mm"}
                    }
                }
            }
        },
        "M18" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "1.50 mm",
            "entries" : {
                "1.50 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "17 mm", "tapDrillDiameter" : "17 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "18 mm"},
                        "75%" : {"holeDiameter" : "16.5 mm", "tapDrillDiameter" : "16.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "18 mm"}
                    }
                },
                "2.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "16.7 mm", "tapDrillDiameter" : "16.7 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "18 mm"},
                        "75%" : {"holeDiameter" : "16 mm", "tapDrillDiameter" : "16 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "18 mm"}
                    }
                },
                "2.5 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "16.4 mm", "tapDrillDiameter" : "16.4 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "18 mm"},
                        "75%" : {"holeDiameter" : "15.5 mm", "tapDrillDiameter" : "15.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "18 mm"}
                    }
                }
            }
        },
        "M20" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "2.50 mm",
            "entries" : {
                "1.50 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "19 mm", "tapDrillDiameter" : "19 mm", "cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "20 mm"},
                        "75%" : {"holeDiameter" : "18.5 mm", "tapDrillDiameter" : "18.5 mm", "cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "20 mm"}
                    }
                },
                "2.00 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "18.5 mm", "tapDrillDiameter" : "18.5 mm", "cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "20 mm"},
                        "75%" : {"holeDiameter" : "18 mm", "tapDrillDiameter" : "18 mm", "cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "20 mm"}
                    }
                },
                "2.50 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "18.5 mm", "tapDrillDiameter" : "18.5 mm", "cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "20 mm"},
                        "75%" : {"holeDiameter" : "17.5 mm", "tapDrillDiameter" : "17.5 mm", "cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "20 mm"}
                    }
                }
            }
        },
        "M22" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "1.50 mm",
            "entries" : {
                "1.50 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "21 mm", "tapDrillDiameter" : "21 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "22 mm"},
                        "75%" : {"holeDiameter" : "20.5 mm", "tapDrillDiameter" : "20.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "22 mm"}
                    }
                },
                "2.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "20.5 mm", "tapDrillDiameter" : "20.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "22 mm"},
                        "75%" : {"holeDiameter" : "20 mm", "tapDrillDiameter" : "20 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "22 mm"}
                    }
                },
                "2.5 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "20.5 mm", "tapDrillDiameter" : "20.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "22 mm"},
                        "75%" : {"holeDiameter" : "19.5 mm", "tapDrillDiameter" : "19.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "22 mm"}
                    }
                }
            }
        },
        "M24" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "2.0 mm",
            "entries" : {
                "2.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "22.5 mm", "tapDrillDiameter" : "22.5 mm", "cBoreDiameter" : "40 mm", "cBoreDepth" : "24.8 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "24 mm"},
                        "75%" : {"holeDiameter" : "22 mm", "tapDrillDiameter" : "22 mm", "cBoreDiameter" : "40 mm", "cBoreDepth" : "24.8 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "24 mm"}
                    }
                },
                "3.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "22 mm", "tapDrillDiameter" : "22 mm", "cBoreDiameter" : "40 mm", "cBoreDepth" : "24.8 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "24 mm"},
                        "75%" : {"holeDiameter" : "21 mm", "tapDrillDiameter" : "21 mm", "cBoreDiameter" : "40 mm", "cBoreDepth" : "24.8 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "24 mm"}
                    }
                }
            }
        },
        "M27" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "2.0 mm",
            "entries" : {
                "2.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "25.5 mm", "tapDrillDiameter" : "25.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "27 mm"},
                        "75%" : {"holeDiameter" : "25 mm", "tapDrillDiameter" : "25 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "27 mm"}
                    }
                },
                "3.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "25 mm", "tapDrillDiameter" : "25 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "27 mm"},
                        "75%" : {"holeDiameter" : "24 mm", "tapDrillDiameter" : "24 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "27 mm"}
                    }
                }
            }
        },
        "M30" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "2.0 mm",
            "entries" : {
                "2.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "28.5 mm", "tapDrillDiameter" : "28.5 mm", "cBoreDiameter" : "50 mm", "cBoreDepth" : "31 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "30 mm"},
                        "75%" : {"holeDiameter" : "28 mm", "tapDrillDiameter" : "28 mm", "cBoreDiameter" : "50 mm", "cBoreDepth" : "31 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "30 mm"}
                    }
                },
                "3.5 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "27.5 mm", "tapDrillDiameter" : "27.5 mm", "cBoreDiameter" : "50 mm", "cBoreDepth" : "31 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "30 mm"},
                        "75%" : {"holeDiameter" : "26.5 mm", "tapDrillDiameter" : "26.5 mm", "cBoreDiameter" : "50 mm", "cBoreDepth" : "31 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "30 mm"}
                    }
                }
            }
        },
        "M33" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "2.0 mm",
            "entries" : {
                "2.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "31.5 mm", "tapDrillDiameter" : "31.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "33 mm"},
                        "75%" : {"holeDiameter" : "31 mm", "tapDrillDiameter" : "31 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "33 mm"}
                    }
                },
                "3.5 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "30.5 mm", "tapDrillDiameter" : "30.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "33 mm"},
                        "75%" : {"holeDiameter" : "29.5 mm", "tapDrillDiameter" : "29.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "33 mm"}
                    }
                }
            }
        },
        "M36" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "3.0 mm",
            "entries" : {
                "3.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "34 mm", "tapDrillDiameter" : "34 mm", "cBoreDiameter" : "58 mm", "cBoreDepth" : "37 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "36 mm"},
                        "75%" : {"holeDiameter" : "33 mm", "tapDrillDiameter" : "33 mm", "cBoreDiameter" : "58 mm", "cBoreDepth" : "37 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "36 mm"}
                    }
                },
                "4.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "33.5 mm", "tapDrillDiameter" : "33.5 mm", "cBoreDiameter" : "58 mm", "cBoreDepth" : "37 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "36 mm"},
                        "75%" : {"holeDiameter" : "32 mm", "tapDrillDiameter" : "32 mm", "cBoreDiameter" : "58 mm", "cBoreDepth" : "37 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "36 mm"}
                    }
                }
            }
        },
        "M39" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "3.0 mm",
            "entries" : {
                "3.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "37 mm", "tapDrillDiameter" : "37 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "39 mm"},
                        "75%" : {"holeDiameter" : "36 mm", "tapDrillDiameter" : "36 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "39 mm"}
                    }
                },
                "4.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "36.5 mm", "tapDrillDiameter" : "36.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "39 mm"},
                        "75%" : {"holeDiameter" : "35 mm", "tapDrillDiameter" : "35 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "39 mm"}
                    }
                }
            }
        },
        "M42" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "3.0 mm",
            "entries" : {
                "3.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "40 mm", "tapDrillDiameter" : "40 mm", "cBoreDiameter" : "69.3 mm", "cBoreDepth" : "43 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "42 mm"},
                        "75%" : {"holeDiameter" : "39 mm", "tapDrillDiameter" : "39 mm", "cBoreDiameter" : "69.3 mm", "cBoreDepth" : "43 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "42 mm"}
                    }
                },
                "4.5 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "39 mm", "tapDrillDiameter" : "39 mm", "cBoreDiameter" : "69.3 mm", "cBoreDepth" : "43 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "42 mm"},
                        "75%" : {"holeDiameter" : "37.5 mm", "tapDrillDiameter" : "37.5 mm", "cBoreDiameter" : "69.3 mm", "cBoreDepth" : "43 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "42 mm"}
                    }
                }
            }
        },
        "M45" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "3.0 mm",
            "entries" : {
                "3.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "43 mm", "tapDrillDiameter" : "43 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "45 mm"},
                        "75%" : {"holeDiameter" : "42 mm", "tapDrillDiameter" : "42 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "45 mm"}
                    }
                },
                "4.5 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "42 mm", "tapDrillDiameter" : "42 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "45 mm"},
                        "75%" : {"holeDiameter" : "40.5 mm", "tapDrillDiameter" : "40.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "45 mm"}
                    }
                }
            }
        },
        "M48" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "3.0 mm",
            "entries" : {
                "3.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "46 mm", "tapDrillDiameter" : "46 mm", "cBoreDiameter" : "79.2 mm", "cBoreDepth" : "49 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "48 mm"},
                        "75%" : {"holeDiameter" : "45 mm", "tapDrillDiameter" : "45 mm", "cBoreDiameter" : "79.2 mm", "cBoreDepth" : "49 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "48 mm"}
                    }
                },
                "5.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "45 mm", "tapDrillDiameter" : "45 mm", "cBoreDiameter" : "79.2 mm", "cBoreDepth" : "49 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "48 mm"},
                        "75%" : {"holeDiameter" : "43 mm", "tapDrillDiameter" : "43 mm", "cBoreDiameter" : "79.2 mm", "cBoreDepth" : "49 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "48 mm"}
                    }
                }
            }
        },
        "M52" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "4.0 mm",
            "entries" : {
                "4.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "49.5 mm", "tapDrillDiameter" : "49.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "52 mm"},
                        "75%" : {"holeDiameter" : "48 mm", "tapDrillDiameter" : "48 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "52 mm"}
                    }
                },
                "5.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "49 mm", "tapDrillDiameter" : "49 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "52 mm"},
                        "75%" : {"holeDiameter" : "47 mm", "tapDrillDiameter" : "47 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "52 mm"}
                    }
                }
            }
        },
        "M56" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "4.0 mm",
            "entries" : {
                "4.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "53.5 mm", "tapDrillDiameter" : "53.5 mm", "cBoreDiameter" : "92.4 mm", "cBoreDepth" : "57 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "56 mm"},
                        "75%" : {"holeDiameter" : "52 mm", "tapDrillDiameter" : "52 mm", "cBoreDiameter" : "92.4 mm", "cBoreDepth" : "57 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "56 mm"}
                    }
                },
                "5.5 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "52.5 mm", "tapDrillDiameter" : "52.5 mm", "cBoreDiameter" : "92.4 mm", "cBoreDepth" : "57 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "56 mm"},
                        "75%" : {"holeDiameter" : "50.5 mm", "tapDrillDiameter" : "50.5 mm", "cBoreDiameter" : "92.4 mm", "cBoreDepth" : "57 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "56 mm"}
                    }
                }
            }
        },
        "M60" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "4.0 mm",
            "entries" : {
                "4.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "57.5 mm", "tapDrillDiameter" : "57.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "60 mm"},
                        "75%" : {"holeDiameter" : "56 mm", "tapDrillDiameter" : "56 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "60 mm"}
                    }
                },
                "5.5 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "56.5 mm", "tapDrillDiameter" : "56.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "60 mm"},
                        "75%" : {"holeDiameter" : "54.5 mm", "tapDrillDiameter" : "54.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "60 mm"}
                    }
                }
            }
        },
        "M64" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "4.0 mm",
            "entries" : {
                "4.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "61.5 mm", "tapDrillDiameter" : "61.5 mm", "cBoreDiameter" : "105.6 mm", "cBoreDepth" : "65 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "64 mm"},
                        "75%" : {"holeDiameter" : "60 mm", "tapDrillDiameter" : "60 mm", "cBoreDiameter" : "105.6 mm", "cBoreDepth" : "65 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "64 mm"}
                    }
                },
                "6.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "60 mm", "tapDrillDiameter" : "60 mm", "cBoreDiameter" : "105.6 mm", "cBoreDepth" : "65 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "64 mm"},
                        "75%" : {"holeDiameter" : "58 mm", "tapDrillDiameter" : "58 mm", "cBoreDiameter" : "105.6 mm", "cBoreDepth" : "65 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "64 mm"}
                    }
                }
            }
        },
        "M68" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "6.0 mm",
            "entries" : {
                "6.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "64 mm", "tapDrillDiameter" : "64 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "68 mm"},
                        "75%" : {"holeDiameter" : "62 mm", "tapDrillDiameter" : "62 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "68 mm"}
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
        "M1" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.20 mm",
            "entries" : {
                "0.20 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1 mm"},
                                "75%" : {"tapDrillDiameter" : "0.8 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.2 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1 mm"},
                                "75%" : {"tapDrillDiameter" : "0.8 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.2 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.3 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1 mm"},
                                "75%" : {"tapDrillDiameter" : "0.8 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.3 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1 mm"}
                            }
                        }
                    }
                },
                "0.25 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.85 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1 mm"},
                                "75%" : {"tapDrillDiameter" : "0.75 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.85 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.2 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1 mm"},
                                "75%" : {"tapDrillDiameter" : "0.75 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.2 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.85 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.3 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1 mm"},
                                "75%" : {"tapDrillDiameter" : "0.75 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.3 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M1.1" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.20 mm",
            "entries" : {
                "0.20 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.2 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.1 mm"},
                                "75%" : {"tapDrillDiameter" : "0.9 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.2 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.1 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.3 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.1 mm"},
                                "75%" : {"tapDrillDiameter" : "0.9 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.3 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.1 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.1 mm"},
                                "75%" : {"tapDrillDiameter" : "0.9 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.1 mm"}
                            }
                        }
                    }
                },
                "0.25 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.95 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.2 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.1 mm"},
                                "75%" : {"tapDrillDiameter" : "0.85 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.2 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.1 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.95 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.3 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.1 mm"},
                                "75%" : {"tapDrillDiameter" : "0.85 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.3 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.1 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.95 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.1 mm"},
                                "75%" : {"tapDrillDiameter" : "0.85 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.1 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M1.2" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.20 mm",
            "entries" : {
                "0.20 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.1 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.3 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.2 mm"},
                                "75%" : {"tapDrillDiameter" : "1.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.3 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.2 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.1 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.2 mm"},
                                "75%" : {"tapDrillDiameter" : "1.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.2 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.1 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.2 mm"},
                                "75%" : {"tapDrillDiameter" : "1.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.2 mm"}
                            }
                        }
                    }
                },
                "0.25 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.3 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.2 mm"},
                                "75%" : {"tapDrillDiameter" : "0.95 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.3 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.2 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.2 mm"},
                                "75%" : {"tapDrillDiameter" : "0.95 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.2 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.2 mm"},
                                "75%" : {"tapDrillDiameter" : "0.95 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.2 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M1.4" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.20 mm",
            "entries" : {
                "0.20 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.3 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.4 mm"},
                                "75%" : {"tapDrillDiameter" : "1.2 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.4 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.3 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.4 mm"},
                                "75%" : {"tapDrillDiameter" : "1.2 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.4 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.3 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.4 mm"},
                                "75%" : {"tapDrillDiameter" : "1.2 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.4 mm"}
                            }
                        }
                    }
                },
                "0.30 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.4 mm"},
                                "75%" : {"tapDrillDiameter" : "1.1 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.4 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.4 mm"},
                                "75%" : {"tapDrillDiameter" : "1.1 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.4 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.4 mm"},
                                "75%" : {"tapDrillDiameter" : "1.1 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.4 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M1.6" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.20 mm",
            "entries" : {
                "0.20 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.5 mm", "cBoreDiameter" : "3.5 mm", "cBoreDepth" : "1.6 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.7 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.6 mm"},
                                "75%" : {"tapDrillDiameter" : "1.4 mm", "cBoreDiameter" : "3.5 mm", "cBoreDepth" : "1.6 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.7 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.6 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.5 mm", "cBoreDiameter" : "3.5 mm", "cBoreDepth" : "1.6 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.6 mm"},
                                "75%" : {"tapDrillDiameter" : "1.4 mm", "cBoreDiameter" : "3.5 mm", "cBoreDepth" : "1.6 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.6 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.5 mm", "cBoreDiameter" : "3.5 mm", "cBoreDepth" : "1.6 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.0 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.6 mm"},
                                "75%" : {"tapDrillDiameter" : "1.4 mm", "cBoreDiameter" : "3.5 mm", "cBoreDepth" : "1.6 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.0 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.6 mm"}
                            }
                        }
                    }
                },
                "0.35 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.35 mm", "cBoreDiameter" : "3.5 mm", "cBoreDepth" : "1.6 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.7 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.6 mm"},
                                "75%" : {"tapDrillDiameter" : "1.25 mm", "cBoreDiameter" : "3.5 mm", "cBoreDepth" : "1.6 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.7 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.6 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.35 mm", "cBoreDiameter" : "3.5 mm", "cBoreDepth" : "1.6 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.6 mm"},
                                "75%" : {"tapDrillDiameter" : "1.25 mm", "cBoreDiameter" : "3.5 mm", "cBoreDepth" : "1.6 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "1.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.6 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.35 mm", "cBoreDiameter" : "3.5 mm", "cBoreDepth" : "1.6 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.0 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.6 mm"},
                                "75%" : {"tapDrillDiameter" : "1.25 mm", "cBoreDiameter" : "3.5 mm", "cBoreDepth" : "1.6 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.0 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.6 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M1.8" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.20 mm",
            "entries" : {
                "0.20 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.7 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.0 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.8 mm"},
                                "75%" : {"tapDrillDiameter" : "1.6 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.0 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.8 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.7 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.8 mm"},
                                "75%" : {"tapDrillDiameter" : "1.6 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.8 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.7 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.2 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.8 mm"},
                                "75%" : {"tapDrillDiameter" : "1.6 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.2 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.8 mm"}
                            }
                        }
                    }
                },
                "0.35 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.55 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.0 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.8 mm"},
                                "75%" : {"tapDrillDiameter" : "1.45 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.0 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.8 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.55 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.8 mm"},
                                "75%" : {"tapDrillDiameter" : "1.45 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.8 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.55 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.2 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.8 mm"},
                                "75%" : {"tapDrillDiameter" : "1.45 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.2 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "1.8 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M2" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.25 mm",
            "entries" : {
                "0.25 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.85 mm", "cBoreDiameter" : "4.4 mm", "cBoreDepth" : "2 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.2 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2 mm"},
                                "75%" : {"tapDrillDiameter" : "1.75 mm", "cBoreDiameter" : "4.4 mm", "cBoreDepth" : "2 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.2 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.85 mm", "cBoreDiameter" : "4.4 mm", "cBoreDepth" : "2 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2 mm"},
                                "75%" : {"tapDrillDiameter" : "1.75 mm", "cBoreDiameter" : "4.4 mm", "cBoreDepth" : "2 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.85 mm", "cBoreDiameter" : "4.4 mm", "cBoreDepth" : "2 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2 mm"},
                                "75%" : {"tapDrillDiameter" : "1.75 mm", "cBoreDiameter" : "4.4 mm", "cBoreDepth" : "2 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2 mm"}
                            }
                        }
                    }
                },
                "0.40 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.75 mm", "cBoreDiameter" : "4.4 mm", "cBoreDepth" : "2 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.2 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2 mm"},
                                "75%" : {"tapDrillDiameter" : "1.6 mm", "cBoreDiameter" : "4.4 mm", "cBoreDepth" : "2 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.2 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.75 mm", "cBoreDiameter" : "4.4 mm", "cBoreDepth" : "2 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2 mm"},
                                "75%" : {"tapDrillDiameter" : "1.6 mm", "cBoreDiameter" : "4.4 mm", "cBoreDepth" : "2 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.75 mm", "cBoreDiameter" : "4.4 mm", "cBoreDepth" : "2 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2 mm"},
                                "75%" : {"tapDrillDiameter" : "1.6 mm", "cBoreDiameter" : "4.4 mm", "cBoreDepth" : "2 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M2.2" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.25 mm",
            "entries" : {
                "0.25 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.2 mm"},
                                "75%" : {"tapDrillDiameter" : "1.95 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.2 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.2 mm"},
                                "75%" : {"tapDrillDiameter" : "1.95 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.2 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.2 mm"},
                                "75%" : {"tapDrillDiameter" : "1.95 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.2 mm"}
                            }
                        }
                    }
                },
                "0.45 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.9 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.2 mm"},
                                "75%" : {"tapDrillDiameter" : "1.75 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.2 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.9 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.2 mm"},
                                "75%" : {"tapDrillDiameter" : "1.75 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.2 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.9 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.2 mm"},
                                "75%" : {"tapDrillDiameter" : "1.75 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.2 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M2.5" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.35 mm",
            "entries" : {
                "0.35 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.3 mm", "cBoreDiameter" : "5.5 mm", "cBoreDepth" : "2.5 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.7 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.5 mm"},
                                "75%" : {"tapDrillDiameter" : "2.15 mm", "cBoreDiameter" : "5.5 mm", "cBoreDepth" : "2.5 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.7 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.5 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.3 mm", "cBoreDiameter" : "5.5 mm", "cBoreDepth" : "2.5 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.9 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.5 mm"},
                                "75%" : {"tapDrillDiameter" : "2.15 mm", "cBoreDiameter" : "5.5 mm", "cBoreDepth" : "2.5 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.9 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.5 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.3 mm", "cBoreDiameter" : "5.5 mm", "cBoreDepth" : "2.5 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "3.1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.5 mm"},
                                "75%" : {"tapDrillDiameter" : "2.15 mm", "cBoreDiameter" : "5.5 mm", "cBoreDepth" : "2.5 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "3.1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.5 mm"}
                            }
                        }
                    }
                },
                "0.45 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.2 mm", "cBoreDiameter" : "5.5 mm", "cBoreDepth" : "2.5 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.7 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.5 mm"},
                                "75%" : {"tapDrillDiameter" : "2.05 mm", "cBoreDiameter" : "5.5 mm", "cBoreDepth" : "2.5 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.7 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.5 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.2 mm", "cBoreDiameter" : "5.5 mm", "cBoreDepth" : "2.5 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.9 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.5 mm"},
                                "75%" : {"tapDrillDiameter" : "2.05 mm", "cBoreDiameter" : "5.5 mm", "cBoreDepth" : "2.5 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "2.9 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.5 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.2 mm", "cBoreDiameter" : "5.5 mm", "cBoreDepth" : "2.5 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "3.1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.5 mm"},
                                "75%" : {"tapDrillDiameter" : "2.05 mm", "cBoreDiameter" : "5.5 mm", "cBoreDepth" : "2.5 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "3.1 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "2.5 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M3" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.50 mm",
            "entries" : {
                "0.35 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.8 mm", "cBoreDiameter" : "6.5 mm", "cBoreDepth" : "3 mm", "cSinkDiameter" : "6.72 mm", "holeDiameter" : "3.2 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "3 mm"},
                                "75%" : {"tapDrillDiameter" : "2.65 mm", "cBoreDiameter" : "6.5 mm", "cBoreDepth" : "3 mm", "cSinkDiameter" : "6.72 mm", "holeDiameter" : "3.2 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "3 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.8 mm", "cBoreDiameter" : "6.5 mm", "cBoreDepth" : "3 mm", "cSinkDiameter" : "6.72 mm", "holeDiameter" : "3.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "3 mm"},
                                "75%" : {"tapDrillDiameter" : "2.65 mm", "cBoreDiameter" : "6.5 mm", "cBoreDepth" : "3 mm", "cSinkDiameter" : "6.72 mm", "holeDiameter" : "3.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "3 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.8 mm", "cBoreDiameter" : "6.5 mm", "cBoreDepth" : "3 mm", "cSinkDiameter" : "6.72 mm", "holeDiameter" : "3.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "3 mm"},
                                "75%" : {"tapDrillDiameter" : "2.65 mm", "cBoreDiameter" : "6.5 mm", "cBoreDepth" : "3 mm", "cSinkDiameter" : "6.72 mm", "holeDiameter" : "3.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "3 mm"}
                            }
                        }
                    }
                },
                "0.50 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.7 mm", "cBoreDiameter" : "6.5 mm", "cBoreDepth" : "3 mm", "cSinkDiameter" : "6.72 mm", "holeDiameter" : "3.2 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "3 mm"},
                                "75%" : {"tapDrillDiameter" : "2.5 mm", "cBoreDiameter" : "6.5 mm", "cBoreDepth" : "3 mm", "cSinkDiameter" : "6.72 mm", "holeDiameter" : "3.2 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "3 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.7 mm", "cBoreDiameter" : "6.5 mm", "cBoreDepth" : "3 mm", "cSinkDiameter" : "6.72 mm", "holeDiameter" : "3.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "3 mm"},
                                "75%" : {"tapDrillDiameter" : "2.5 mm", "cBoreDiameter" : "6.5 mm", "cBoreDepth" : "3 mm", "cSinkDiameter" : "6.72 mm", "holeDiameter" : "3.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "3 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.7 mm", "cBoreDiameter" : "6.5 mm", "cBoreDepth" : "3 mm", "cSinkDiameter" : "6.72 mm", "holeDiameter" : "3.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "3 mm"},
                                "75%" : {"tapDrillDiameter" : "2.5 mm", "cBoreDiameter" : "6.5 mm", "cBoreDepth" : "3 mm", "cSinkDiameter" : "6.72 mm", "holeDiameter" : "3.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "3 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M4" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.70 mm",
            "entries" : {
                "0.50 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.7 mm", "cBoreDiameter" : "8.25 mm", "cBoreDepth" : "4 mm", "cSinkDiameter" : "8.96 mm", "holeDiameter" : "4.3 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4 mm"},
                                "75%" : {"tapDrillDiameter" : "3.5 mm", "cBoreDiameter" : "8.25 mm", "cBoreDepth" : "4 mm", "cSinkDiameter" : "8.96 mm", "holeDiameter" : "4.3 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.7 mm", "cBoreDiameter" : "8.25 mm", "cBoreDepth" : "4 mm", "cSinkDiameter" : "8.96 mm", "holeDiameter" : "4.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4 mm"},
                                "75%" : {"tapDrillDiameter" : "3.5 mm", "cBoreDiameter" : "8.25 mm", "cBoreDepth" : "4 mm", "cSinkDiameter" : "8.96 mm", "holeDiameter" : "4.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.7 mm", "cBoreDiameter" : "8.25 mm", "cBoreDepth" : "4 mm", "cSinkDiameter" : "8.96 mm", "holeDiameter" : "4.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4 mm"},
                                "75%" : {"tapDrillDiameter" : "3.5 mm", "cBoreDiameter" : "8.25 mm", "cBoreDepth" : "4 mm", "cSinkDiameter" : "8.96 mm", "holeDiameter" : "4.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4 mm"}
                            }
                        }
                    }
                },
                "0.70 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.5 mm", "cBoreDiameter" : "8.25 mm", "cBoreDepth" : "4 mm", "cSinkDiameter" : "8.96 mm", "holeDiameter" : "4.3 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4 mm"},
                                "75%" : {"tapDrillDiameter" : "3.3 mm", "cBoreDiameter" : "8.25 mm", "cBoreDepth" : "4 mm", "cSinkDiameter" : "8.96 mm", "holeDiameter" : "4.3 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.5 mm", "cBoreDiameter" : "8.25 mm", "cBoreDepth" : "4 mm", "cSinkDiameter" : "8.96 mm", "holeDiameter" : "4.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4 mm"},
                                "75%" : {"tapDrillDiameter" : "3.3 mm", "cBoreDiameter" : "8.25 mm", "cBoreDepth" : "4 mm", "cSinkDiameter" : "8.96 mm", "holeDiameter" : "4.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.5 mm", "cBoreDiameter" : "8.25 mm", "cBoreDepth" : "4 mm", "cSinkDiameter" : "8.96 mm", "holeDiameter" : "4.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4 mm"},
                                "75%" : {"tapDrillDiameter" : "3.3 mm", "cBoreDiameter" : "8.25 mm", "cBoreDepth" : "4 mm", "cSinkDiameter" : "8.96 mm", "holeDiameter" : "4.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M4.5" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.50 mm",
            "entries" : {
                "0.50 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "4.2 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "4.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4.5 mm"},
                                "75%" : {"tapDrillDiameter" : "4.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "4.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4.5 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "4.2 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "5.0 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4.5 mm"},
                                "75%" : {"tapDrillDiameter" : "4.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "5.0 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4.5 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "4.2 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "5.3 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4.5 mm"},
                                "75%" : {"tapDrillDiameter" : "4.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "5.3 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4.5 mm"}
                            }
                        }
                    }
                },
                "0.75 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "4.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "4.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4.5 mm"},
                                "75%" : {"tapDrillDiameter" : "3.7 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "4.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4.5 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "4.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "5.0 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4.5 mm"},
                                "75%" : {"tapDrillDiameter" : "3.7 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "5.0 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4.5 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "4.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "5.3 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4.5 mm"},
                                "75%" : {"tapDrillDiameter" : "3.7 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "5.3 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "4.5 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M5" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.80 mm",
            "entries" : {
                "0.5 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "4.7 mm", "cBoreDiameter" : "9.75 mm", "cBoreDepth" : "5 mm", "cSinkDiameter" : "11.2 mm", "holeDiameter" : "5.3 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "5 mm"},
                                "75%" : {"tapDrillDiameter" : "4.5 mm", "cBoreDiameter" : "9.75 mm", "cBoreDepth" : "5 mm", "cSinkDiameter" : "11.2 mm", "holeDiameter" : "5.3 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "5 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "4.7 mm", "cBoreDiameter" : "9.75 mm", "cBoreDepth" : "5 mm", "cSinkDiameter" : "11.2 mm", "holeDiameter" : "5.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "5 mm"},
                                "75%" : {"tapDrillDiameter" : "4.5 mm", "cBoreDiameter" : "9.75 mm", "cBoreDepth" : "5 mm", "cSinkDiameter" : "11.2 mm", "holeDiameter" : "5.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "5 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "4.7 mm", "cBoreDiameter" : "9.75 mm", "cBoreDepth" : "5 mm", "cSinkDiameter" : "11.2 mm", "holeDiameter" : "5.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "5 mm"},
                                "75%" : {"tapDrillDiameter" : "4.5 mm", "cBoreDiameter" : "9.75 mm", "cBoreDepth" : "5 mm", "cSinkDiameter" : "11.2 mm", "holeDiameter" : "5.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "5 mm"}
                            }
                        }
                    }
                },
                "0.80 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "4.5 mm", "cBoreDiameter" : "9.75 mm", "cBoreDepth" : "5 mm", "cSinkDiameter" : "11.2 mm", "holeDiameter" : "5.3 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "5 mm"},
                                "75%" : {"tapDrillDiameter" : "4.2 mm", "cBoreDiameter" : "9.75 mm", "cBoreDepth" : "5 mm", "cSinkDiameter" : "11.2 mm", "holeDiameter" : "5.3 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "5 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "4.5 mm", "cBoreDiameter" : "9.75 mm", "cBoreDepth" : "5 mm", "cSinkDiameter" : "11.2 mm", "holeDiameter" : "5.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "5 mm"},
                                "75%" : {"tapDrillDiameter" : "4.2 mm", "cBoreDiameter" : "9.75 mm", "cBoreDepth" : "5 mm", "cSinkDiameter" : "11.2 mm", "holeDiameter" : "5.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "5 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "4.5 mm", "cBoreDiameter" : "9.75 mm", "cBoreDepth" : "5 mm", "cSinkDiameter" : "11.2 mm", "holeDiameter" : "5.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "5 mm"},
                                "75%" : {"tapDrillDiameter" : "4.2 mm", "cBoreDiameter" : "9.75 mm", "cBoreDepth" : "5 mm", "cSinkDiameter" : "11.2 mm", "holeDiameter" : "5.8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "5 mm"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "5.5 mm", "cBoreDiameter" : "11.25 mm", "cBoreDepth" : "6 mm", "cSinkDiameter" : "13.44 mm", "holeDiameter" : "6.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "6 mm"},
                                "75%" : {"tapDrillDiameter" : "5.25 mm", "cBoreDiameter" : "11.25 mm", "cBoreDepth" : "6 mm", "cSinkDiameter" : "13.44 mm", "holeDiameter" : "6.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "6 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "5.5 mm", "cBoreDiameter" : "11.25 mm", "cBoreDepth" : "6 mm", "cSinkDiameter" : "13.44 mm", "holeDiameter" : "6.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "6 mm"},
                                "75%" : {"tapDrillDiameter" : "5.25 mm", "cBoreDiameter" : "11.25 mm", "cBoreDepth" : "6 mm", "cSinkDiameter" : "13.44 mm", "holeDiameter" : "6.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "6 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "5.5 mm", "cBoreDiameter" : "11.25 mm", "cBoreDepth" : "6 mm", "cSinkDiameter" : "13.44 mm", "holeDiameter" : "7 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "6 mm"},
                                "75%" : {"tapDrillDiameter" : "5.25 mm", "cBoreDiameter" : "11.25 mm", "cBoreDepth" : "6 mm", "cSinkDiameter" : "13.44 mm", "holeDiameter" : "7 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "6 mm"}
                            }
                        }
                    }
                },
                "1.00 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "5.4 mm", "cBoreDiameter" : "11.25 mm", "cBoreDepth" : "6 mm", "cSinkDiameter" : "13.44 mm", "holeDiameter" : "6.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "6 mm"},
                                "75%" : {"tapDrillDiameter" : "5 mm", "cBoreDiameter" : "11.25 mm", "cBoreDepth" : "6 mm", "cSinkDiameter" : "13.44 mm", "holeDiameter" : "6.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "6 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "5.4 mm", "cBoreDiameter" : "11.25 mm", "cBoreDepth" : "6 mm", "cSinkDiameter" : "13.44 mm", "holeDiameter" : "6.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "6 mm"},
                                "75%" : {"tapDrillDiameter" : "5 mm", "cBoreDiameter" : "11.25 mm", "cBoreDepth" : "6 mm", "cSinkDiameter" : "13.44 mm", "holeDiameter" : "6.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "6 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "5.4 mm", "cBoreDiameter" : "11.25 mm", "cBoreDepth" : "6 mm", "cSinkDiameter" : "13.44 mm", "holeDiameter" : "7 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "6 mm"},
                                "75%" : {"tapDrillDiameter" : "5 mm", "cBoreDiameter" : "11.25 mm", "cBoreDepth" : "6 mm", "cSinkDiameter" : "13.44 mm", "holeDiameter" : "7 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "6 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M7" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "0.75 mm",
            "entries" : {
                "0.75 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "6.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "7.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "7 mm"},
                                "75%" : {"tapDrillDiameter" : "6.2 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "7.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "7 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "6.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "7.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "7 mm"},
                                "75%" : {"tapDrillDiameter" : "6.2 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "7.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "7 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "6.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "7 mm"},
                                "75%" : {"tapDrillDiameter" : "6.2 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "7 mm"}
                            }
                        }
                    }
                },
                "1.0 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "6.4 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "7.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "7 mm"},
                                "75%" : {"tapDrillDiameter" : "6.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "7.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "7 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "6.4 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "7.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "7 mm"},
                                "75%" : {"tapDrillDiameter" : "6.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "7.6 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "7 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "6.4 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "7 mm"},
                                "75%" : {"tapDrillDiameter" : "6.0 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "8 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "7 mm"}
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
                "0.75 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "7.5 mm", "cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "holeDiameter" : "8.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "8 mm"},
                                "75%" : {"tapDrillDiameter" : "7.2 mm", "cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "holeDiameter" : "8.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "8 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "7.5 mm", "cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "holeDiameter" : "9 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "8 mm"},
                                "75%" : {"tapDrillDiameter" : "7.2 mm", "cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "holeDiameter" : "9 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "8 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "7.5 mm", "cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "holeDiameter" : "10 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "8 mm"},
                                "75%" : {"tapDrillDiameter" : "7.2 mm", "cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "holeDiameter" : "10 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "8 mm"}
                            }
                        }
                    }
                },
                "1.00 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "7.4 mm", "cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "holeDiameter" : "8.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "8 mm"},
                                "75%" : {"tapDrillDiameter" : "7 mm", "cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "holeDiameter" : "8.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "8 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "7.4 mm", "cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "holeDiameter" : "9 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "8 mm"},
                                "75%" : {"tapDrillDiameter" : "7 mm", "cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "holeDiameter" : "9 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "8 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "7.4 mm", "cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "holeDiameter" : "10 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "8 mm"},
                                "75%" : {"tapDrillDiameter" : "7 mm", "cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "holeDiameter" : "10 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "8 mm"}
                            }
                        }
                    }
                },
                "1.25 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "7.2 mm", "cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "holeDiameter" : "8.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "8 mm"},
                                "75%" : {"tapDrillDiameter" : "6.8 mm", "cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "holeDiameter" : "8.4 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "8 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "7.2 mm", "cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "holeDiameter" : "9 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "8 mm"},
                                "75%" : {"tapDrillDiameter" : "6.8 mm", "cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "holeDiameter" : "9 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "8 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "7.2 mm", "cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "holeDiameter" : "10 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "8 mm"},
                                "75%" : {"tapDrillDiameter" : "6.8 mm", "cBoreDiameter" : "14.25 mm", "cBoreDepth" : "8 mm", "cSinkDiameter" : "17.92 mm", "holeDiameter" : "10 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "8 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M10" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "1.50 mm",
            "entries" : {
                "1.00 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "9.4 mm", "cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "holeDiameter" : "10.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "10 mm"},
                                "75%" : {"tapDrillDiameter" : "9 mm", "cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "holeDiameter" : "10.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "10 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "9.4 mm", "cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "holeDiameter" : "11 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "10 mm"},
                                "75%" : {"tapDrillDiameter" : "9 mm", "cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "holeDiameter" : "11 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "10 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "9.4 mm", "cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "holeDiameter" : "12 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "10 mm"},
                                "75%" : {"tapDrillDiameter" : "9 mm", "cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "holeDiameter" : "12 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "10 mm"}
                            }
                        }
                    }
                },
                "1.25 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "9.2 mm", "cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "holeDiameter" : "10.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "10 mm"},
                                "75%" : {"tapDrillDiameter" : "8.8 mm", "cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "holeDiameter" : "10.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "10 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "9.2 mm", "cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "holeDiameter" : "11 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "10 mm"},
                                "75%" : {"tapDrillDiameter" : "8.8 mm", "cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "holeDiameter" : "11 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "10 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "9.2 mm", "cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "holeDiameter" : "12 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "10 mm"},
                                "75%" : {"tapDrillDiameter" : "8.8 mm", "cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "holeDiameter" : "12 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "10 mm"}
                            }
                        }
                    }
                },
                "1.50 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "9 mm", "cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "holeDiameter" : "10.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "10 mm"},
                                "75%" : {"tapDrillDiameter" : "8.5 mm", "cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "holeDiameter" : "10.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "10 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "9 mm", "cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "holeDiameter" : "11 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "10 mm"},
                                "75%" : {"tapDrillDiameter" : "8.5 mm", "cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "holeDiameter" : "11 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "10 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "9 mm", "cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "holeDiameter" : "12 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "10 mm"},
                                "75%" : {"tapDrillDiameter" : "8.5 mm", "cBoreDiameter" : "17.25 mm", "cBoreDepth" : "10 mm", "cSinkDiameter" : "22.4 mm", "holeDiameter" : "12 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "10 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M12" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "1.75 mm",
            "entries" : {
                "1.25 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "11.2 mm", "cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "holeDiameter" : "13 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "12 mm"},
                                "75%" : {"tapDrillDiameter" : "10.8 mm", "cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "holeDiameter" : "13 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "12 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "11.2 mm", "cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "holeDiameter" : "13.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "12 mm"},
                                "75%" : {"tapDrillDiameter" : "10.8 mm", "cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "holeDiameter" : "13.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "12 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "11.2 mm", "cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "holeDiameter" : "14.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "12 mm"},
                                "75%" : {"tapDrillDiameter" : "10.8 mm", "cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "holeDiameter" : "14.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "12 mm"}
                            }
                        }
                    }
                },
                "1.50 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "11 mm", "cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "holeDiameter" : "13 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "12 mm"},
                                "75%" : {"tapDrillDiameter" : "10.5 mm", "cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "holeDiameter" : "13 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "12 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "11 mm", "cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "holeDiameter" : "13.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "12 mm"},
                                "75%" : {"tapDrillDiameter" : "10.5 mm", "cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "holeDiameter" : "13.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "12 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "11 mm", "cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "holeDiameter" : "14.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "12 mm"},
                                "75%" : {"tapDrillDiameter" : "10.5 mm", "cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "holeDiameter" : "14.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "12 mm"}
                            }
                        }
                    }
                },
                "1.75 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "11.2 mm", "cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "holeDiameter" : "13 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "12 mm"},
                                "75%" : {"tapDrillDiameter" : "10.3 mm", "cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "holeDiameter" : "13 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "12 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "11.2 mm", "cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "holeDiameter" : "13.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "12 mm"},
                                "75%" : {"tapDrillDiameter" : "10.3 mm", "cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "holeDiameter" : "13.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "12 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "11.2 mm", "cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "holeDiameter" : "14.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "12 mm"},
                                "75%" : {"tapDrillDiameter" : "10.3 mm", "cBoreDiameter" : "19.25 mm", "cBoreDepth" : "12 mm", "cSinkDiameter" : "26.88 mm", "holeDiameter" : "14.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "12 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M14" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "2.00 mm",
            "entries" : {
                "1.25 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "13.2 mm", "cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "holeDiameter" : "15 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "14 mm"},
                                "75%" : {"tapDrillDiameter" : "12.8 mm", "cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "holeDiameter" : "15 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "14 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "13.2 mm", "cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "holeDiameter" : "15.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "14 mm"},
                                "75%" : {"tapDrillDiameter" : "12.8 mm", "cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "holeDiameter" : "15.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "14 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "13.2 mm", "cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "holeDiameter" : "16.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "14 mm"},
                                "75%" : {"tapDrillDiameter" : "12.8 mm", "cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "holeDiameter" : "16.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "14 mm"}
                            }
                        }
                    }
                },
                "1.50 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "13 mm", "cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "holeDiameter" : "15 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "14 mm"},
                                "75%" : {"tapDrillDiameter" : "12.5 mm", "cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "holeDiameter" : "15 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "14 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "13 mm", "cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "holeDiameter" : "15.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "14 mm"},
                                "75%" : {"tapDrillDiameter" : "12.5 mm", "cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "holeDiameter" : "15.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "14 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "13 mm", "cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "holeDiameter" : "16.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "14 mm"},
                                "75%" : {"tapDrillDiameter" : "12.5 mm", "cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "holeDiameter" : "16.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "14 mm"}
                            }
                        }
                    }
                },
                "2.00 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "12.7 mm", "cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "holeDiameter" : "15 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "14 mm"},
                                "75%" : {"tapDrillDiameter" : "12.1 mm", "cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "holeDiameter" : "15 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "14 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "12.7 mm", "cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "holeDiameter" : "15.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "14 mm"},
                                "75%" : {"tapDrillDiameter" : "12.1 mm", "cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "holeDiameter" : "15.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "14 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "12.7 mm", "cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "holeDiameter" : "16.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "14 mm"},
                                "75%" : {"tapDrillDiameter" : "12.1 mm", "cBoreDiameter" : "22.25 mm", "cBoreDepth" : "14 mm", "cSinkDiameter" : "30.8 mm", "holeDiameter" : "16.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "14 mm"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "15 mm", "cBoreDiameter" : "25.5 mm", "cBoreDepth" : "16 mm", "cSinkDiameter" : "33.6 mm", "holeDiameter" : "17 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "16 mm"},
                                "75%" : {"tapDrillDiameter" : "14.5 mm", "cBoreDiameter" : "25.5 mm", "cBoreDepth" : "16 mm", "cSinkDiameter" : "33.6 mm", "holeDiameter" : "17 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "16 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "15 mm", "cBoreDiameter" : "25.5 mm", "cBoreDepth" : "16 mm", "cSinkDiameter" : "33.6 mm", "holeDiameter" : "17.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "16 mm"},
                                "75%" : {"tapDrillDiameter" : "14.5 mm", "cBoreDiameter" : "25.5 mm", "cBoreDepth" : "16 mm", "cSinkDiameter" : "33.6 mm", "holeDiameter" : "17.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "16 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "15 mm", "cBoreDiameter" : "25.5 mm", "cBoreDepth" : "16 mm", "cSinkDiameter" : "33.6 mm", "holeDiameter" : "18.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "16 mm"},
                                "75%" : {"tapDrillDiameter" : "14.5 mm", "cBoreDiameter" : "25.5 mm", "cBoreDepth" : "16 mm", "cSinkDiameter" : "33.6 mm", "holeDiameter" : "18.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "16 mm"}
                            }
                        }
                    }
                },
                "2.00 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "14.75 mm", "cBoreDiameter" : "25.5 mm", "cBoreDepth" : "16 mm", "cSinkDiameter" : "33.6 mm", "holeDiameter" : "17 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "16 mm"},
                                "75%" : {"tapDrillDiameter" : "14 mm", "cBoreDiameter" : "25.5 mm", "cBoreDepth" : "16 mm", "cSinkDiameter" : "33.6 mm", "holeDiameter" : "17 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "16 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "14.75 mm", "cBoreDiameter" : "25.5 mm", "cBoreDepth" : "16 mm", "cSinkDiameter" : "33.6 mm", "holeDiameter" : "17.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "16 mm"},
                                "75%" : {"tapDrillDiameter" : "14 mm", "cBoreDiameter" : "25.5 mm", "cBoreDepth" : "16 mm", "cSinkDiameter" : "33.6 mm", "holeDiameter" : "17.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "16 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "14.75 mm", "cBoreDiameter" : "25.5 mm", "cBoreDepth" : "16 mm", "cSinkDiameter" : "33.6 mm", "holeDiameter" : "18.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "16 mm"},
                                "75%" : {"tapDrillDiameter" : "14 mm", "cBoreDiameter" : "25.5 mm", "cBoreDepth" : "16 mm", "cSinkDiameter" : "33.6 mm", "holeDiameter" : "18.5 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "16 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M18" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "1.50 mm",
            "entries" : {
                "1.50 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "17 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "19 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "18 mm"},
                                "75%" : {"tapDrillDiameter" : "16.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "19 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "18 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "17 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "20 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "18 mm"},
                                "75%" : {"tapDrillDiameter" : "16.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "20 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "18 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "17 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "21 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "18 mm"},
                                "75%" : {"tapDrillDiameter" : "16.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "21 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "18 mm"}
                            }
                        }
                    }
                },
                "2.0 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "16.7 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "19 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "18 mm"},
                                "75%" : {"tapDrillDiameter" : "16 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "19 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "18 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "16.7 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "20 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "18 mm"},
                                "75%" : {"tapDrillDiameter" : "16 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "20 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "18 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "16.7 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "21 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "18 mm"},
                                "75%" : {"tapDrillDiameter" : "16 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "21 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "18 mm"}
                            }
                        }
                    }
                },
                "2.5 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "16.4 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "19 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "18 mm"},
                                "75%" : {"tapDrillDiameter" : "15.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "19 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "18 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "16.4 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "20 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "18 mm"},
                                "75%" : {"tapDrillDiameter" : "15.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "20 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "18 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "16.4 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "21 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "18 mm"},
                                "75%" : {"tapDrillDiameter" : "15.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "21 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "18 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M20" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "2.50 mm",
            "entries" : {
                "1.50 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "19 mm", "cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "holeDiameter" : "21 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "20 mm"},
                                "75%" : {"tapDrillDiameter" : "18.5 mm", "cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "holeDiameter" : "21 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "20 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "19 mm", "cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "holeDiameter" : "22 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "20 mm"},
                                "75%" : {"tapDrillDiameter" : "18.5 mm", "cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "holeDiameter" : "22 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "20 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "19 mm", "cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "holeDiameter" : "24 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "20 mm"},
                                "75%" : {"tapDrillDiameter" : "18.5 mm", "cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "holeDiameter" : "24 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "20 mm"}
                            }
                        }
                    }
                },
                "2.00 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "18.5 mm", "cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "holeDiameter" : "21 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "20 mm"},
                                "75%" : {"tapDrillDiameter" : "18 mm", "cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "holeDiameter" : "21 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "20 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "18.5 mm", "cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "holeDiameter" : "22 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "20 mm"},
                                "75%" : {"tapDrillDiameter" : "18 mm", "cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "holeDiameter" : "22 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "20 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "18.5 mm", "cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "holeDiameter" : "24 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "20 mm"},
                                "75%" : {"tapDrillDiameter" : "18 mm", "cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "holeDiameter" : "24 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "20 mm"}
                            }
                        }
                    }
                },
                "2.50 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "18.5 mm", "cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "holeDiameter" : "21 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "20 mm"},
                                "75%" : {"tapDrillDiameter" : "17.5 mm", "cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "holeDiameter" : "21 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "20 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "18.5 mm", "cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "holeDiameter" : "22 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "20 mm"},
                                "75%" : {"tapDrillDiameter" : "17.5 mm", "cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "holeDiameter" : "22 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "20 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "18.5 mm", "cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "holeDiameter" : "24 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "20 mm"},
                                "75%" : {"tapDrillDiameter" : "17.5 mm", "cBoreDiameter" : "31.5 mm", "cBoreDepth" : "20 mm", "cSinkDiameter" : "42 mm", "holeDiameter" : "24 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "20 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M22" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "1.50 mm",
            "entries" : {
                "1.50 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "21 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "23 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "22 mm"},
                                "75%" : {"tapDrillDiameter" : "20.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "23 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "22 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "21 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "24 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "22 mm"},
                                "75%" : {"tapDrillDiameter" : "20.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "24 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "22 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "21 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "26 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "22 mm"},
                                "75%" : {"tapDrillDiameter" : "20.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "26 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "22 mm"}
                            }
                        }
                    }
                },
                "2.0 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "20.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "23 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "22 mm"},
                                "75%" : {"tapDrillDiameter" : "20 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "23 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "22 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "20.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "24 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "22 mm"},
                                "75%" : {"tapDrillDiameter" : "20 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "24 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "22 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "20.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "26 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "22 mm"},
                                "75%" : {"tapDrillDiameter" : "20 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "26 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "22 mm"}
                            }
                        }
                    }
                },
                "2.5 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "20.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "23 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "22 mm"},
                                "75%" : {"tapDrillDiameter" : "19.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "23 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "22 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "20.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "24 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "22 mm"},
                                "75%" : {"tapDrillDiameter" : "19.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "24 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "22 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "20.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "26 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "22 mm"},
                                "75%" : {"tapDrillDiameter" : "19.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "26 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "22 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M24" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "2.0 mm",
            "entries" : {
                "2.0 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "22.5 mm", "cBoreDiameter" : "40 mm", "cBoreDepth" : "24.8 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "25 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "24 mm"},
                                "75%" : {"tapDrillDiameter" : "22 mm", "cBoreDiameter" : "40 mm", "cBoreDepth" : "24.8 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "25 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "24 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "22.5 mm", "cBoreDiameter" : "40 mm", "cBoreDepth" : "24.8 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "26 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "24 mm"},
                                "75%" : {"tapDrillDiameter" : "22 mm", "cBoreDiameter" : "40 mm", "cBoreDepth" : "24.8 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "26 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "24 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "22.5 mm", "cBoreDiameter" : "40 mm", "cBoreDepth" : "24.8 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "28 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "24 mm"},
                                "75%" : {"tapDrillDiameter" : "22 mm", "cBoreDiameter" : "40 mm", "cBoreDepth" : "24.8 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "28 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "24 mm"}
                            }
                        }
                    }
                },
                "3.0 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "22 mm", "cBoreDiameter" : "40 mm", "cBoreDepth" : "24.8 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "25 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "24 mm"},
                                "75%" : {"tapDrillDiameter" : "21 mm", "cBoreDiameter" : "40 mm", "cBoreDepth" : "24.8 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "25 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "24 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "22 mm", "cBoreDiameter" : "40 mm", "cBoreDepth" : "24.8 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "26 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "24 mm"},
                                "75%" : {"tapDrillDiameter" : "21 mm", "cBoreDiameter" : "40 mm", "cBoreDepth" : "24.8 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "26 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "24 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "22 mm", "cBoreDiameter" : "40 mm", "cBoreDepth" : "24.8 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "28 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "24 mm"},
                                "75%" : {"tapDrillDiameter" : "21 mm", "cBoreDiameter" : "40 mm", "cBoreDepth" : "24.8 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "28 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "24 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M27" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "2.0 mm",
            "entries" : {
                "2.0 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "25.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "28 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "27 mm"},
                                "75%" : {"tapDrillDiameter" : "25 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "28 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "27 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "25.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "30 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "27 mm"},
                                "75%" : {"tapDrillDiameter" : "25 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "30 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "27 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "25.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "32 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "27 mm"},
                                "75%" : {"tapDrillDiameter" : "25 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "32 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "27 mm"}
                            }
                        }
                    }
                },
                "3.0 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "25 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "28 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "27 mm"},
                                "75%" : {"tapDrillDiameter" : "24 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "28 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "27 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "25 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "30 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "27 mm"},
                                "75%" : {"tapDrillDiameter" : "24 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "30 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "27 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "25 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "32 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "27 mm"},
                                "75%" : {"tapDrillDiameter" : "24 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "32 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "27 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M30" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "2.0 mm",
            "entries" : {
                "2.0 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "28.5 mm", "cBoreDiameter" : "50 mm", "cBoreDepth" : "31 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "31 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "30 mm"},
                                "75%" : {"tapDrillDiameter" : "28 mm", "cBoreDiameter" : "50 mm", "cBoreDepth" : "31 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "31 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "30 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "28.5 mm", "cBoreDiameter" : "50 mm", "cBoreDepth" : "31 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "33 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "30 mm"},
                                "75%" : {"tapDrillDiameter" : "28 mm", "cBoreDiameter" : "50 mm", "cBoreDepth" : "31 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "33 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "30 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "28.5 mm", "cBoreDiameter" : "50 mm", "cBoreDepth" : "31 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "35 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "30 mm"},
                                "75%" : {"tapDrillDiameter" : "28 mm", "cBoreDiameter" : "50 mm", "cBoreDepth" : "31 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "35 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "30 mm"}
                            }
                        }
                    }
                },
                "3.5 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "27.5 mm", "cBoreDiameter" : "50 mm", "cBoreDepth" : "31 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "31 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "30 mm"},
                                "75%" : {"tapDrillDiameter" : "26.5 mm", "cBoreDiameter" : "50 mm", "cBoreDepth" : "31 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "31 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "30 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "27.5 mm", "cBoreDiameter" : "50 mm", "cBoreDepth" : "31 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "33 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "30 mm"},
                                "75%" : {"tapDrillDiameter" : "26.5 mm", "cBoreDiameter" : "50 mm", "cBoreDepth" : "31 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "33 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "30 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "27.5 mm", "cBoreDiameter" : "50 mm", "cBoreDepth" : "31 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "35 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "30 mm"},
                                "75%" : {"tapDrillDiameter" : "26.5 mm", "cBoreDiameter" : "50 mm", "cBoreDepth" : "31 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "35 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "30 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M33" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "2.0 mm",
            "entries" : {
                "2.0 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "31.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "34 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "33 mm"},
                                "75%" : {"tapDrillDiameter" : "31 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "34 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "33 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "31.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "36 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "33 mm"},
                                "75%" : {"tapDrillDiameter" : "31 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "36 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "33 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "31.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "38 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "33 mm"},
                                "75%" : {"tapDrillDiameter" : "31 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "38 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "33 mm"}
                            }
                        }
                    }
                },
                "3.5 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "30.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "34 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "33 mm"},
                                "75%" : {"tapDrillDiameter" : "29.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "34 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "33 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "30.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "36 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "33 mm"},
                                "75%" : {"tapDrillDiameter" : "29.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "36 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "33 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "30.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "38 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "33 mm"},
                                "75%" : {"tapDrillDiameter" : "29.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "38 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "33 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M36" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "3.0 mm",
            "entries" : {
                "3.0 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "34 mm", "cBoreDiameter" : "58 mm", "cBoreDepth" : "37 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "37 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "36 mm"},
                                "75%" : {"tapDrillDiameter" : "33 mm", "cBoreDiameter" : "58 mm", "cBoreDepth" : "37 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "37 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "36 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "34 mm", "cBoreDiameter" : "58 mm", "cBoreDepth" : "37 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "39 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "36 mm"},
                                "75%" : {"tapDrillDiameter" : "33 mm", "cBoreDiameter" : "58 mm", "cBoreDepth" : "37 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "39 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "36 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "34 mm", "cBoreDiameter" : "58 mm", "cBoreDepth" : "37 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "42 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "36 mm"},
                                "75%" : {"tapDrillDiameter" : "33 mm", "cBoreDiameter" : "58 mm", "cBoreDepth" : "37 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "42 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "36 mm"}
                            }
                        }
                    }
                },
                "4.0 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "33.5 mm", "cBoreDiameter" : "58 mm", "cBoreDepth" : "37 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "37 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "36 mm"},
                                "75%" : {"tapDrillDiameter" : "32 mm", "cBoreDiameter" : "58 mm", "cBoreDepth" : "37 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "37 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "36 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "33.5 mm", "cBoreDiameter" : "58 mm", "cBoreDepth" : "37 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "39 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "36 mm"},
                                "75%" : {"tapDrillDiameter" : "32 mm", "cBoreDiameter" : "58 mm", "cBoreDepth" : "37 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "39 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "36 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "33.5 mm", "cBoreDiameter" : "58 mm", "cBoreDepth" : "37 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "42 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "36 mm"},
                                "75%" : {"tapDrillDiameter" : "32 mm", "cBoreDiameter" : "58 mm", "cBoreDepth" : "37 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "42 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "36 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M39" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "3.0 mm",
            "entries" : {
                "3.0 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "37 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "40 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "39 mm"},
                                "75%" : {"tapDrillDiameter" : "36 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "40 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "39 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "37 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "42 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "39 mm"},
                                "75%" : {"tapDrillDiameter" : "36 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "42 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "39 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "37 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "45 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "39 mm"},
                                "75%" : {"tapDrillDiameter" : "36 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "45 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "39 mm"}
                            }
                        }
                    }
                },
                "4.0 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "36.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "40 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "39 mm"},
                                "75%" : {"tapDrillDiameter" : "35 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "40 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "39 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "36.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "42 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "39 mm"},
                                "75%" : {"tapDrillDiameter" : "35 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "42 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "39 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "36.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "45 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "39 mm"},
                                "75%" : {"tapDrillDiameter" : "35 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "45 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "39 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M42" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "3.0 mm",
            "entries" : {
                "3.0 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "40 mm", "cBoreDiameter" : "69.3 mm", "cBoreDepth" : "43 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "43 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "42 mm"},
                                "75%" : {"tapDrillDiameter" : "39 mm", "cBoreDiameter" : "69.3 mm", "cBoreDepth" : "43 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "43 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "42 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "40 mm", "cBoreDiameter" : "69.3 mm", "cBoreDepth" : "43 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "45 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "42 mm"},
                                "75%" : {"tapDrillDiameter" : "39 mm", "cBoreDiameter" : "69.3 mm", "cBoreDepth" : "43 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "45 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "42 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "40 mm", "cBoreDiameter" : "69.3 mm", "cBoreDepth" : "43 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "48 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "42 mm"},
                                "75%" : {"tapDrillDiameter" : "39 mm", "cBoreDiameter" : "69.3 mm", "cBoreDepth" : "43 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "48 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "42 mm"}
                            }
                        }
                    }
                },
                "4.5 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "39 mm", "cBoreDiameter" : "69.3 mm", "cBoreDepth" : "43 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "43 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "42 mm"},
                                "75%" : {"tapDrillDiameter" : "37.5 mm", "cBoreDiameter" : "69.3 mm", "cBoreDepth" : "43 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "43 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "42 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "39 mm", "cBoreDiameter" : "69.3 mm", "cBoreDepth" : "43 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "45 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "42 mm"},
                                "75%" : {"tapDrillDiameter" : "37.5 mm", "cBoreDiameter" : "69.3 mm", "cBoreDepth" : "43 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "45 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "42 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "39 mm", "cBoreDiameter" : "69.3 mm", "cBoreDepth" : "43 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "48 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "42 mm"},
                                "75%" : {"tapDrillDiameter" : "37.5 mm", "cBoreDiameter" : "69.3 mm", "cBoreDepth" : "43 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "48 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "42 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M45" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "3.0 mm",
            "entries" : {
                "3.0 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "43 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "46 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "45 mm"},
                                "75%" : {"tapDrillDiameter" : "42 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "46 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "45 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "43 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "48 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "45 mm"},
                                "75%" : {"tapDrillDiameter" : "42 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "48 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "45 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "43 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "52 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "45 mm"},
                                "75%" : {"tapDrillDiameter" : "42 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "52 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "45 mm"}
                            }
                        }
                    }
                },
                "4.5 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "42 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "46 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "45 mm"},
                                "75%" : {"tapDrillDiameter" : "40.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "46 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "45 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "42 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "48 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "45 mm"},
                                "75%" : {"tapDrillDiameter" : "40.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "48 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "45 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "42 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "52 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "45 mm"},
                                "75%" : {"tapDrillDiameter" : "40.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "52 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "45 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M48" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "3.0 mm",
            "entries" : {
                "3.0 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "46 mm", "cBoreDiameter" : "79.2 mm", "cBoreDepth" : "49 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "50 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "48 mm"},
                                "75%" : {"tapDrillDiameter" : "45 mm", "cBoreDiameter" : "79.2 mm", "cBoreDepth" : "49 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "50 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "48 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "46 mm", "cBoreDiameter" : "79.2 mm", "cBoreDepth" : "49 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "52 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "48 mm"},
                                "75%" : {"tapDrillDiameter" : "45 mm", "cBoreDiameter" : "79.2 mm", "cBoreDepth" : "49 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "52 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "48 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "46 mm", "cBoreDiameter" : "79.2 mm", "cBoreDepth" : "49 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "56 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "48 mm"},
                                "75%" : {"tapDrillDiameter" : "45 mm", "cBoreDiameter" : "79.2 mm", "cBoreDepth" : "49 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "56 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "48 mm"}
                            }
                        }
                    }
                },
                "5.0 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "45 mm", "cBoreDiameter" : "79.2 mm", "cBoreDepth" : "49 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "50 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "48 mm"},
                                "75%" : {"tapDrillDiameter" : "43 mm", "cBoreDiameter" : "79.2 mm", "cBoreDepth" : "49 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "50 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "48 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "45 mm", "cBoreDiameter" : "79.2 mm", "cBoreDepth" : "49 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "52 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "48 mm"},
                                "75%" : {"tapDrillDiameter" : "43 mm", "cBoreDiameter" : "79.2 mm", "cBoreDepth" : "49 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "52 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "48 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "45 mm", "cBoreDiameter" : "79.2 mm", "cBoreDepth" : "49 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "56 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "48 mm"},
                                "75%" : {"tapDrillDiameter" : "43 mm", "cBoreDiameter" : "79.2 mm", "cBoreDepth" : "49 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "56 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "48 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M52" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "4.0 mm",
            "entries" : {
                "4.0 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "49.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "54 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "52 mm"},
                                "75%" : {"tapDrillDiameter" : "48 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "54 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "52 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "49.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "56 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "52 mm"},
                                "75%" : {"tapDrillDiameter" : "48 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "56 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "52 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "49.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "62 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "52 mm"},
                                "75%" : {"tapDrillDiameter" : "48 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "62 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "52 mm"}
                            }
                        }
                    }
                },
                "5.0 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "49 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "54 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "52 mm"},
                                "75%" : {"tapDrillDiameter" : "47 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "54 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "52 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "49 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "56 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "52 mm"},
                                "75%" : {"tapDrillDiameter" : "47 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "56 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "52 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "49 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "62 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "52 mm"},
                                "75%" : {"tapDrillDiameter" : "47 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "62 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "52 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M56" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "4.0 mm",
            "entries" : {
                "4.0 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "53.5 mm", "cBoreDiameter" : "92.4 mm", "cBoreDepth" : "57 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "58 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "56 mm"},
                                "75%" : {"tapDrillDiameter" : "52 mm", "cBoreDiameter" : "92.4 mm", "cBoreDepth" : "57 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "58 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "56 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "53.5 mm", "cBoreDiameter" : "92.4 mm", "cBoreDepth" : "57 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "62 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "56 mm"},
                                "75%" : {"tapDrillDiameter" : "52 mm", "cBoreDiameter" : "92.4 mm", "cBoreDepth" : "57 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "62 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "56 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "53.5 mm", "cBoreDiameter" : "92.4 mm", "cBoreDepth" : "57 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "66 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "56 mm"},
                                "75%" : {"tapDrillDiameter" : "52 mm", "cBoreDiameter" : "92.4 mm", "cBoreDepth" : "57 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "66 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "56 mm"}
                            }
                        }
                    }
                },
                "5.5 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "52.5 mm", "cBoreDiameter" : "92.4 mm", "cBoreDepth" : "57 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "58 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "56 mm"},
                                "75%" : {"tapDrillDiameter" : "50.5 mm", "cBoreDiameter" : "92.4 mm", "cBoreDepth" : "57 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "58 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "56 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "52.5 mm", "cBoreDiameter" : "92.4 mm", "cBoreDepth" : "57 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "62 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "56 mm"},
                                "75%" : {"tapDrillDiameter" : "50.5 mm", "cBoreDiameter" : "92.4 mm", "cBoreDepth" : "57 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "62 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "56 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "52.5 mm", "cBoreDiameter" : "92.4 mm", "cBoreDepth" : "57 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "66 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "56 mm"},
                                "75%" : {"tapDrillDiameter" : "50.5 mm", "cBoreDiameter" : "92.4 mm", "cBoreDepth" : "57 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "66 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "56 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M60" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "4.0 mm",
            "entries" : {
                "4.0 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "57.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "62 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "60 mm"},
                                "75%" : {"tapDrillDiameter" : "56 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "62 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "60 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "57.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "66 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "60 mm"},
                                "75%" : {"tapDrillDiameter" : "56 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "66 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "60 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "57.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "70 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "60 mm"},
                                "75%" : {"tapDrillDiameter" : "56 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "70 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "60 mm"}
                            }
                        }
                    }
                },
                "5.5 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "56.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "62 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "60 mm"},
                                "75%" : {"tapDrillDiameter" : "54.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "62 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "60 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "56.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "66 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "60 mm"},
                                "75%" : {"tapDrillDiameter" : "54.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "66 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "60 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "56.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "70 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "60 mm"},
                                "75%" : {"tapDrillDiameter" : "54.5 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "70 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "60 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M64" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "4.0 mm",
            "entries" : {
                "4.0 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "61.5 mm", "cBoreDiameter" : "105.6 mm", "cBoreDepth" : "65 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "66 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "64 mm"},
                                "75%" : {"tapDrillDiameter" : "60 mm", "cBoreDiameter" : "105.6 mm", "cBoreDepth" : "65 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "66 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "64 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "61.5 mm", "cBoreDiameter" : "105.6 mm", "cBoreDepth" : "65 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "70 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "64 mm"},
                                "75%" : {"tapDrillDiameter" : "60 mm", "cBoreDiameter" : "105.6 mm", "cBoreDepth" : "65 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "70 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "64 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "61.5 mm", "cBoreDiameter" : "105.6 mm", "cBoreDepth" : "65 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "74 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "64 mm"},
                                "75%" : {"tapDrillDiameter" : "60 mm", "cBoreDiameter" : "105.6 mm", "cBoreDepth" : "65 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "74 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "64 mm"}
                            }
                        }
                    }
                },
                "6.0 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "60 mm", "cBoreDiameter" : "105.6 mm", "cBoreDepth" : "65 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "66 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "64 mm"},
                                "75%" : {"tapDrillDiameter" : "58 mm", "cBoreDiameter" : "105.6 mm", "cBoreDepth" : "65 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "66 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "64 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "60 mm", "cBoreDiameter" : "105.6 mm", "cBoreDepth" : "65 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "70 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "64 mm"},
                                "75%" : {"tapDrillDiameter" : "58 mm", "cBoreDiameter" : "105.6 mm", "cBoreDepth" : "65 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "70 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "64 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "60 mm", "cBoreDiameter" : "105.6 mm", "cBoreDepth" : "65 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "74 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "64 mm"},
                                "75%" : {"tapDrillDiameter" : "58 mm", "cBoreDiameter" : "105.6 mm", "cBoreDepth" : "65 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "74 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "64 mm"}
                            }
                        }
                    }
                }
            }
        },
        "M68" : {
            "name" : "pitch",
            "displayName" : "Pitch",
            "default" : "6.0 mm",
            "entries" : {
                "6.0 mm" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "64 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "70 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "68 mm"},
                                "75%" : {"tapDrillDiameter" : "62 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "70 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "68 mm"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "64 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "74 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "68 mm"},
                                "75%" : {"tapDrillDiameter" : "62 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "74 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "68 mm"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "64 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "78 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "68 mm"},
                                "75%" : {"tapDrillDiameter" : "62 mm", "cBoreDiameter" : "-1 mm", "cBoreDepth" : "0 mm", "cSinkDiameter" : "-1 mm", "holeDiameter" : "78 mm", "cSinkAngle" : "90 degree", "majorDiameter" : "68 mm"}
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


