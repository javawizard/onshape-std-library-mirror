FeatureScript 626; /* Automatically generated version */
/* Automatically generated file -- DO NOT EDIT */

import(path : "onshape/std/units.fs", version : "626.0");
import(path : "onshape/std/lookupTablePath.fs", version : "626.0");

const ANSI_drillTable = {
    "name" : "size",
    "displayName" : "Drill size",
    "default" : "1/4 (0.25)",
    "entries" : {
        "#80 (0.0135)" : {"holeDiameter" : "0.0135 * inch", "tapDrillDiameter" : "0.0135 * inch"},
        "#79 (0.0145)" : {"holeDiameter" : "0.0145 * inch", "tapDrillDiameter" : "0.0145 * inch"},
        "1/64 (0.0156)" : {"holeDiameter" : "1/64 * inch", "tapDrillDiameter" : "1/64 * inch"},
        "#78 (0.016)" : {"holeDiameter" : "0.016 * inch", "tapDrillDiameter" : "0.016 * inch"},
        "#77 (0.018)" : {"holeDiameter" : "0.018 * inch", "tapDrillDiameter" : "0.018 * inch"},
        "#76 (0.02)" : {"holeDiameter" : "0.02 * inch", "tapDrillDiameter" : "0.02 * inch"},
        "#75 (0.021)" : {"holeDiameter" : "0.021 * inch", "tapDrillDiameter" : "0.021 * inch"},
        "#74 (0.0225)" : {"holeDiameter" : "0.0225 * inch", "tapDrillDiameter" : "0.0225 * inch"},
        "#73 (0.024)" : {"holeDiameter" : "0.024 * inch", "tapDrillDiameter" : "0.024 * inch"},
        "#72 (0.025)" : {"holeDiameter" : "0.025 * inch", "tapDrillDiameter" : "0.025 * inch"},
        "#71 (0.026)" : {"holeDiameter" : "0.026 * inch", "tapDrillDiameter" : "0.026 * inch"},
        "#70 (0.028)" : {"holeDiameter" : "0.028 * inch", "tapDrillDiameter" : "0.028 * inch"},
        "#69 (0.0292)" : {"holeDiameter" : "0.0292 * inch", "tapDrillDiameter" : "0.0292 * inch"},
        "#68 (0.031)" : {"holeDiameter" : "0.031 * inch", "tapDrillDiameter" : "0.031 * inch"},
        "1/32 (0.0312)" : {"holeDiameter" : "1/32 * inch", "tapDrillDiameter" : "1/32 * inch"},
        "#67 (0.032)" : {"holeDiameter" : "0.032 * inch", "tapDrillDiameter" : "0.032 * inch"},
        "#66 (0.033)" : {"holeDiameter" : "0.033 * inch", "tapDrillDiameter" : "0.033 * inch"},
        "#65 (0.035)" : {"holeDiameter" : "0.035 * inch", "tapDrillDiameter" : "0.035 * inch"},
        "#64 (0.036)" : {"holeDiameter" : "0.036 * inch", "tapDrillDiameter" : "0.036 * inch"},
        "#63 (0.037)" : {"holeDiameter" : "0.037 * inch", "tapDrillDiameter" : "0.037 * inch"},
        "#62 (0.038)" : {"holeDiameter" : "0.038 * inch", "tapDrillDiameter" : "0.038 * inch"},
        "#61 (0.039)" : {"holeDiameter" : "0.039 * inch", "tapDrillDiameter" : "0.039 * inch"},
        "#60 (0.04)" : {"holeDiameter" : "0.04 * inch", "tapDrillDiameter" : "0.04 * inch"},
        "#59 (0.041)" : {"holeDiameter" : "0.041 * inch", "tapDrillDiameter" : "0.041 * inch"},
        "#58 (0.042)" : {"holeDiameter" : "0.042 * inch", "tapDrillDiameter" : "0.042 * inch"},
        "#57 (0.043)" : {"holeDiameter" : "0.043 * inch", "tapDrillDiameter" : "0.043 * inch"},
        "#56 (0.0465)" : {"holeDiameter" : "0.0465 * inch", "tapDrillDiameter" : "0.0465 * inch"},
        "3/64 (0.0469)" : {"holeDiameter" : "3/64 * inch", "tapDrillDiameter" : "3/64 * inch"},
        "#55 (0.052)" : {"holeDiameter" : "0.052 * inch", "tapDrillDiameter" : "0.052 * inch"},
        "#54 (0.055)" : {"holeDiameter" : "0.055 * inch", "tapDrillDiameter" : "0.055 * inch"},
        "#53 (0.0595)" : {"holeDiameter" : "0.0595 * inch", "tapDrillDiameter" : "0.0595 * inch"},
        "1/16 (0.0625)" : {"holeDiameter" : "1/16 * inch", "tapDrillDiameter" : "1/16 * inch"},
        "#52 (0.0635)" : {"holeDiameter" : "0.0635 * inch", "tapDrillDiameter" : "0.0635 * inch"},
        "#51 (0.067)" : {"holeDiameter" : "0.067 * inch", "tapDrillDiameter" : "0.067 * inch"},
        "#50 (0.07)" : {"holeDiameter" : "0.07 * inch", "tapDrillDiameter" : "0.07 * inch"},
        "#49 (0.073)" : {"holeDiameter" : "0.073 * inch", "tapDrillDiameter" : "0.073 * inch"},
        "#48 (0.076)" : {"holeDiameter" : "0.076 * inch", "tapDrillDiameter" : "0.076 * inch"},
        "5/64 (0.0781)" : {"holeDiameter" : "5/64 * inch", "tapDrillDiameter" : "5/64 * inch"},
        "#47 (0.0785)" : {"holeDiameter" : "0.0785 * inch", "tapDrillDiameter" : "0.0785 * inch"},
        "#46 (0.081)" : {"holeDiameter" : "0.081 * inch", "tapDrillDiameter" : "0.081 * inch"},
        "#45 (0.082)" : {"holeDiameter" : "0.082 * inch", "tapDrillDiameter" : "0.082 * inch"},
        "#44 (0.086)" : {"holeDiameter" : "0.086 * inch", "tapDrillDiameter" : "0.086 * inch"},
        "#43 (0.089)" : {"holeDiameter" : "0.089 * inch", "tapDrillDiameter" : "0.089 * inch"},
        "#42 (0.0935)" : {"holeDiameter" : "0.0935 * inch", "tapDrillDiameter" : "0.0935 * inch"},
        "3/32 (0.0937)" : {"holeDiameter" : "3/32 * inch", "tapDrillDiameter" : "3/32 * inch"},
        "#41 (0.096)" : {"holeDiameter" : "0.096 * inch", "tapDrillDiameter" : "0.096 * inch"},
        "#40 (0.098)" : {"holeDiameter" : "0.098 * inch", "tapDrillDiameter" : "0.098 * inch"},
        "#39 (0.0995)" : {"holeDiameter" : "0.0995 * inch", "tapDrillDiameter" : "0.0995 * inch"},
        "#38 (0.1015)" : {"holeDiameter" : "0.1015 * inch", "tapDrillDiameter" : "0.1015 * inch"},
        "#37 (0.104)" : {"holeDiameter" : "0.104 * inch", "tapDrillDiameter" : "0.104 * inch"},
        "#36 (0.1065)" : {"holeDiameter" : "0.1065 * inch", "tapDrillDiameter" : "0.1065 * inch"},
        "7/64 (0.1094)" : {"holeDiameter" : "7/64 * inch", "tapDrillDiameter" : "7/64 * inch"},
        "#35 (0.11)" : {"holeDiameter" : "0.11 * inch", "tapDrillDiameter" : "0.11 * inch"},
        "#34 (0.111)" : {"holeDiameter" : "0.111 * inch", "tapDrillDiameter" : "0.111 * inch"},
        "#33 (0.113)" : {"holeDiameter" : "0.113 * inch", "tapDrillDiameter" : "0.113 * inch"},
        "#32 (0.116)" : {"holeDiameter" : "0.116 * inch", "tapDrillDiameter" : "0.116 * inch"},
        "#31 (0.12)" : {"holeDiameter" : "0.12 * inch", "tapDrillDiameter" : "0.12 * inch"},
        "1/8 (0.125)" : {"holeDiameter" : "1/8 * inch", "tapDrillDiameter" : "1/8 * inch"},
        "#30 (0.1285)" : {"holeDiameter" : "0.1285 * inch", "tapDrillDiameter" : "0.1285 * inch"},
        "#29 (0.136)" : {"holeDiameter" : "0.136 * inch", "tapDrillDiameter" : "0.136 * inch"},
        "#28 (0.1405)" : {"holeDiameter" : "0.1405 * inch", "tapDrillDiameter" : "0.1405 * inch"},
        "9/64 (0.1406)" : {"holeDiameter" : "9/64 * inch", "tapDrillDiameter" : "9/64 * inch"},
        "#27 (0.144)" : {"holeDiameter" : "0.144 * inch", "tapDrillDiameter" : "0.144 * inch"},
        "#26 (0.147)" : {"holeDiameter" : "0.147 * inch", "tapDrillDiameter" : "0.147 * inch"},
        "#25 (0.1495)" : {"holeDiameter" : "0.1495 * inch", "tapDrillDiameter" : "0.1495 * inch"},
        "#24 (0.152)" : {"holeDiameter" : "0.152 * inch", "tapDrillDiameter" : "0.152 * inch"},
        "#23 (0.154)" : {"holeDiameter" : "0.154 * inch", "tapDrillDiameter" : "0.154 * inch"},
        "5/32 (0.1562)" : {"holeDiameter" : "5/32 * inch", "tapDrillDiameter" : "5/32 * inch"},
        "#22 (0.157)" : {"holeDiameter" : "0.157 * inch", "tapDrillDiameter" : "0.157 * inch"},
        "#21 (0.159)" : {"holeDiameter" : "0.159 * inch", "tapDrillDiameter" : "0.159 * inch"},
        "#20 (0.161)" : {"holeDiameter" : "0.161 * inch", "tapDrillDiameter" : "0.161 * inch"},
        "#19 (0.166)" : {"holeDiameter" : "0.166 * inch", "tapDrillDiameter" : "0.166 * inch"},
        "#18 (0.1695)" : {"holeDiameter" : "0.1695 * inch", "tapDrillDiameter" : "0.1695 * inch"},
        "11/64 (0.1719)" : {"holeDiameter" : "11/64 * inch", "tapDrillDiameter" : "11/64 * inch"},
        "#17 (0.173)" : {"holeDiameter" : "0.173 * inch", "tapDrillDiameter" : "0.173 * inch"},
        "#16 (0.177)" : {"holeDiameter" : "0.177 * inch", "tapDrillDiameter" : "0.177 * inch"},
        "#15 (0.18)" : {"holeDiameter" : "0.18 * inch", "tapDrillDiameter" : "0.18 * inch"},
        "#14 (0.182)" : {"holeDiameter" : "0.182 * inch", "tapDrillDiameter" : "0.182 * inch"},
        "#13 (0.185)" : {"holeDiameter" : "0.185 * inch", "tapDrillDiameter" : "0.185 * inch"},
        "3/16 (0.1875)" : {"holeDiameter" : "3/16 * inch", "tapDrillDiameter" : "3/16 * inch"},
        "#12 (0.189)" : {"holeDiameter" : "0.189 * inch", "tapDrillDiameter" : "0.189 * inch"},
        "#11 (0.191)" : {"holeDiameter" : "0.191 * inch", "tapDrillDiameter" : "0.191 * inch"},
        "#10 (0.1935)" : {"holeDiameter" : "0.1935 * inch", "tapDrillDiameter" : "0.1935 * inch"},
        "#9 (0.196)" : {"holeDiameter" : "0.196 * inch", "tapDrillDiameter" : "0.196 * inch"},
        "#8 (0.199)" : {"holeDiameter" : "0.199 * inch", "tapDrillDiameter" : "0.199 * inch"},
        "#7 (0.201)" : {"holeDiameter" : "0.201 * inch", "tapDrillDiameter" : "0.201 * inch"},
        "13/64 (0.2031)" : {"holeDiameter" : "13/64 * inch", "tapDrillDiameter" : "13/64 * inch"},
        "#6 (0.204)" : {"holeDiameter" : "0.204 * inch", "tapDrillDiameter" : "0.204 * inch"},
        "#5 (0.2055)" : {"holeDiameter" : "0.2055 * inch", "tapDrillDiameter" : "0.2055 * inch"},
        "#4 (0.209)" : {"holeDiameter" : "0.209 * inch", "tapDrillDiameter" : "0.209 * inch"},
        "#3 (0.213)" : {"holeDiameter" : "0.213 * inch", "tapDrillDiameter" : "0.213 * inch"},
        "7/32 (0.2187)" : {"holeDiameter" : "7/32 * inch", "tapDrillDiameter" : "7/32 * inch"},
        "#2 (0.221)" : {"holeDiameter" : "0.221 * inch", "tapDrillDiameter" : "0.221 * inch"},
        "#1 (0.228)" : {"holeDiameter" : "0.228 * inch", "tapDrillDiameter" : "0.228 * inch"},
        "A (0.234)" : {"holeDiameter" : "0.234 * inch", "tapDrillDiameter" : "0.234 * inch"},
        "15/64 (0.2344)" : {"holeDiameter" : "15/64 * inch", "tapDrillDiameter" : "15/64 * inch"},
        "B (0.238)" : {"holeDiameter" : "0.238 * inch", "tapDrillDiameter" : "0.238 * inch"},
        "C (0.242)" : {"holeDiameter" : "0.242 * inch", "tapDrillDiameter" : "0.242 * inch"},
        "D (0.246)" : {"holeDiameter" : "0.246 * inch", "tapDrillDiameter" : "0.246 * inch"},
        "1/4 (0.25)" : {"holeDiameter" : "1/4 * inch", "tapDrillDiameter" : "1/4 * inch"},
        "E (0.25)" : {"holeDiameter" : "0.25 * inch", "tapDrillDiameter" : "0.25 * inch"},
        "F (0.257)" : {"holeDiameter" : "0.257 * inch", "tapDrillDiameter" : "0.257 * inch"},
        "G (0.261)" : {"holeDiameter" : "0.261 * inch", "tapDrillDiameter" : "0.261 * inch"},
        "17/64 (0.2656)" : {"holeDiameter" : "17/64 * inch", "tapDrillDiameter" : "17/64 * inch"},
        "H (0.266)" : {"holeDiameter" : "0.266 * inch", "tapDrillDiameter" : "0.266 * inch"},
        "I (0.272)" : {"holeDiameter" : "0.272 * inch", "tapDrillDiameter" : "0.272 * inch"},
        "J (0.277)" : {"holeDiameter" : "0.277 * inch", "tapDrillDiameter" : "0.277 * inch"},
        "K (0.2811)" : {"holeDiameter" : "0.2811 * inch", "tapDrillDiameter" : "0.2811 * inch"},
        "9/32 (0.2812)" : {"holeDiameter" : "9/32 * inch", "tapDrillDiameter" : "9/32 * inch"},
        "L (0.29)" : {"holeDiameter" : "0.29 * inch", "tapDrillDiameter" : "0.29 * inch"},
        "M (0.295)" : {"holeDiameter" : "0.295 * inch", "tapDrillDiameter" : "0.295 * inch"},
        "19/64 (0.2968)" : {"holeDiameter" : "19/64 * inch", "tapDrillDiameter" : "19/64 * inch"},
        "N (0.302)" : {"holeDiameter" : "0.302 * inch", "tapDrillDiameter" : "0.302 * inch"},
        "5/16 (0.3125)" : {"holeDiameter" : "5/16 * inch", "tapDrillDiameter" : "5/16 * inch"},
        "O (0.316)" : {"holeDiameter" : "0.316 * inch", "tapDrillDiameter" : "0.316 * inch"},
        "P (0.323)" : {"holeDiameter" : "0.323 * inch", "tapDrillDiameter" : "0.323 * inch"},
        "21/64 (0.3281)" : {"holeDiameter" : "21/64 * inch", "tapDrillDiameter" : "21/64 * inch"},
        "Q (0.332)" : {"holeDiameter" : "0.332 * inch", "tapDrillDiameter" : "0.332 * inch"},
        "R (0.339)" : {"holeDiameter" : "0.339 * inch", "tapDrillDiameter" : "0.339 * inch"},
        "11/32 (0.3437)" : {"holeDiameter" : "11/32 * inch", "tapDrillDiameter" : "11/32 * inch"},
        "S (0.348)" : {"holeDiameter" : "0.348 * inch", "tapDrillDiameter" : "0.348 * inch"},
        "T (0.358)" : {"holeDiameter" : "0.358 * inch", "tapDrillDiameter" : "0.358 * inch"},
        "23/64 (0.3594)" : {"holeDiameter" : "23/64 * inch", "tapDrillDiameter" : "23/64 * inch"},
        "U (0.368)" : {"holeDiameter" : "0.368 * inch", "tapDrillDiameter" : "0.368 * inch"},
        "3/8 (0.375)" : {"holeDiameter" : "3/8 * inch", "tapDrillDiameter" : "3/8 * inch"},
        "V (0.377)" : {"holeDiameter" : "0.377 * inch", "tapDrillDiameter" : "0.377 * inch"},
        "W (0.386)" : {"holeDiameter" : "0.386 * inch", "tapDrillDiameter" : "0.386 * inch"},
        "25/64 (0.3906)" : {"holeDiameter" : "25/64 * inch", "tapDrillDiameter" : "25/64 * inch"},
        "X (0.397)" : {"holeDiameter" : "0.397 * inch", "tapDrillDiameter" : "0.397 * inch"},
        "Y (0.404)" : {"holeDiameter" : "0.404 * inch", "tapDrillDiameter" : "0.404 * inch"},
        "13/32 (0.4062)" : {"holeDiameter" : "13/32 * inch", "tapDrillDiameter" : "13/32 * inch"},
        "Z (0.413)" : {"holeDiameter" : "0.413 * inch", "tapDrillDiameter" : "0.413 * inch"},
        "27/64 (0.4219)" : {"holeDiameter" : "27/64 * inch", "tapDrillDiameter" : "27/64 * inch"},
        "7/16 (0.4375)" : {"holeDiameter" : "7/16 * inch", "tapDrillDiameter" : "7/16 * inch"},
        "29/64 (0.4531)" : {"holeDiameter" : "29/64 * inch", "tapDrillDiameter" : "29/64 * inch"},
        "15/32 (0.4687)" : {"holeDiameter" : "15/32 * inch", "tapDrillDiameter" : "15/32 * inch"},
        "31/64 (0.4844)" : {"holeDiameter" : "31/64 * inch", "tapDrillDiameter" : "31/64 * inch"},
        "1/2 (0.5)" : {"holeDiameter" : "1/2 * inch", "tapDrillDiameter" : "1/2 * inch"},
        "33/64 (0.5156)" : {"holeDiameter" : "33/64 * inch", "tapDrillDiameter" : "33/64 * inch"},
        "17/32 (0.5312)" : {"holeDiameter" : "17/32 * inch", "tapDrillDiameter" : "17/32 * inch"},
        "35/64 (0.5469)" : {"holeDiameter" : "35/64 * inch", "tapDrillDiameter" : "35/64 * inch"},
        "9/16 (0.5625)" : {"holeDiameter" : "9/16 * inch", "tapDrillDiameter" : "9/16 * inch"},
        "37/64 (0.5781)" : {"holeDiameter" : "37/64 * inch", "tapDrillDiameter" : "37/64 * inch"},
        "19/32 (0.5937)" : {"holeDiameter" : "19/32 * inch", "tapDrillDiameter" : "19/32 * inch"},
        "39/64 (0.6094)" : {"holeDiameter" : "39/64 * inch", "tapDrillDiameter" : "39/64 * inch"},
        "5/8 (0.625)" : {"holeDiameter" : "5/8 * inch", "tapDrillDiameter" : "5/8 * inch"},
        "41/64 (0.6406)" : {"holeDiameter" : "41/64 * inch", "tapDrillDiameter" : "41/64 * inch"},
        "21/32 (0.6562)" : {"holeDiameter" : "21/32 * inch", "tapDrillDiameter" : "21/32 * inch"},
        "43/64 (0.6719)" : {"holeDiameter" : "43/64 * inch", "tapDrillDiameter" : "43/64 * inch"},
        "11/16 (0.6875)" : {"holeDiameter" : "11/16 * inch", "tapDrillDiameter" : "11/16 * inch"},
        "45/64 (0.7031)" : {"holeDiameter" : "45/64 * inch", "tapDrillDiameter" : "45/64 * inch"},
        "23/32 (0.7187)" : {"holeDiameter" : "23/32 * inch", "tapDrillDiameter" : "23/32 * inch"},
        "47/64 (0.7344)" : {"holeDiameter" : "47/64 * inch", "tapDrillDiameter" : "47/64 * inch"},
        "3/4 (0.75)" : {"holeDiameter" : "3/4 * inch", "tapDrillDiameter" : "3/4 * inch"},
        "49/64 (0.7656)" : {"holeDiameter" : "49/64 * inch", "tapDrillDiameter" : "49/64 * inch"},
        "25/32 (0.7812)" : {"holeDiameter" : "25/32 * inch", "tapDrillDiameter" : "25/32 * inch"},
        "51/64 (0.7969)" : {"holeDiameter" : "51/64 * inch", "tapDrillDiameter" : "51/64 * inch"},
        "13/16 (0.8125)" : {"holeDiameter" : "13/16 * inch", "tapDrillDiameter" : "13/16 * inch"},
        "53/64 (0.8281)" : {"holeDiameter" : "53/64 * inch", "tapDrillDiameter" : "53/64 * inch"},
        "27/32 (0.8437)" : {"holeDiameter" : "27/32 * inch", "tapDrillDiameter" : "27/32 * inch"},
        "55/64 (0.8594)" : {"holeDiameter" : "55/64 * inch", "tapDrillDiameter" : "55/64 * inch"},
        "7/8 (0.875)" : {"holeDiameter" : "7/8 * inch", "tapDrillDiameter" : "7/8 * inch"},
        "57/64 (0.8906)" : {"holeDiameter" : "57/64 * inch", "tapDrillDiameter" : "57/64 * inch"},
        "29/32 (0.9062)" : {"holeDiameter" : "29/32 * inch", "tapDrillDiameter" : "29/32 * inch"},
        "59/64 (0.9219)" : {"holeDiameter" : "59/64 * inch", "tapDrillDiameter" : "59/64 * inch"},
        "15/16 (0.9375)" : {"holeDiameter" : "15/16 * inch", "tapDrillDiameter" : "15/16 * inch"},
        "61/64 (0.9531)" : {"holeDiameter" : "61/64 * inch", "tapDrillDiameter" : "61/64 * inch"},
        "31/32 (0.9687)" : {"holeDiameter" : "31/32 * inch", "tapDrillDiameter" : "31/32 * inch"},
        "63/64 (0.9844)" : {"holeDiameter" : "63/64 * inch", "tapDrillDiameter" : "63/64 * inch"},
        "1 (1.0)" : {"holeDiameter" : "1 * inch", "tapDrillDiameter" : "1 * inch"}
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
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "0.067 * inch", "cBoreDiameter" : "1/8 * inch", "cBoreDepth" : "0.06 * inch", "cSinkDiameter" : "0.138 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "0.094 * inch", "cBoreDiameter" : "1/8 * inch", "cBoreDepth" : "0.06 * inch", "cSinkDiameter" : "0.138 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "0.076 * inch", "cBoreDiameter" : "1/8 * inch", "cBoreDepth" : "0.06 * inch", "cSinkDiameter" : "0.138 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "#1" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "0.081 * inch", "cBoreDiameter" : "5/32 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "0.104 * inch", "cBoreDiameter" : "5/32 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "0.089 * inch", "cBoreDiameter" : "5/32 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "#2" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "0.094 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "0.116 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "0.102 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "#3" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "0.106 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "0.128 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "0.116 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "#4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "0.120 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "0.144 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "0.128 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "#5" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "0.141 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "0.172 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "0.156 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "#6" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "0.154 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "0.185 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "0.170 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "#8" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "0.180 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "0.213 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "0.196 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "#10" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "0.206 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "0.238 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "0.221 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "#12" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "0.221 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "0.246 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "0.228 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "1/4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "0.266 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "0.297 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "0.281 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "5/16" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "0.328 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "0.359 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "0.344 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "3/8" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "0.391 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "0.422 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "0.406 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "7/16" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "0.453 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "0.484 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "0.469 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "1/2" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "0.531 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "0.609 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "0.562 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "9/16" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "0.5781 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "0.6094 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "0.5938 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "5/8" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "0.656 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "0.734 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "0.688 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "3/4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "0.781 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "0.906 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "0.812 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "7/8" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "0.906 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "1.031 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "0.938 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "1" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "1.031 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "1.156 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "1.094 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "1 1/8" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "1.1406 * inch", "cBoreDiameter" : "1.8125 * inch", "cBoreDepth" : "1.125 * inch", "cSinkDiameter" : "2.188 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "1.1719 * inch", "cBoreDiameter" : "1.8125 * inch", "cBoreDepth" : "1.125 * inch", "cSinkDiameter" : "2.188 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "1.1562 * inch", "cBoreDiameter" : "1.8125 * inch", "cBoreDepth" : "1.125 * inch", "cSinkDiameter" : "2.188 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "1 1/4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "1.281 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "1.438 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "1.344 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "1 3/8" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "1.3906 * inch", "cBoreDiameter" : "2.1875 * inch", "cBoreDepth" : "1.375 * inch", "cSinkDiameter" : "2.688 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "1.4219 * inch", "cBoreDiameter" : "2.1875 * inch", "cBoreDepth" : "1.375 * inch", "cSinkDiameter" : "2.688 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "1.4062 * inch", "cBoreDiameter" : "2.1875 * inch", "cBoreDepth" : "1.375 * inch", "cSinkDiameter" : "2.688 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "1 1/2" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "1.562 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "1.734 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "1.625 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "1 3/4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "1.7656 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "1.7969 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "1.7812 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "2" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "2.0156 * inch", "cBoreDiameter" : "3.125 * inch", "cBoreDepth" : "2.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "2.0469 * inch", "cBoreDiameter" : "3.125 * inch", "cBoreDepth" : "2.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "2.0312 * inch", "cBoreDiameter" : "3.125 * inch", "cBoreDepth" : "2.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "2 1/4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "2.2656 * inch", "cBoreDiameter" : "3.5 * inch", "cBoreDepth" : "2.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "2.2969 * inch", "cBoreDiameter" : "3.5 * inch", "cBoreDepth" : "2.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "2.2812 * inch", "cBoreDiameter" : "3.5 * inch", "cBoreDepth" : "2.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "2 1/2" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "2.5156 * inch", "cBoreDiameter" : "3.875 * inch", "cBoreDepth" : "2.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "2.5469 * inch", "cBoreDiameter" : "3.875 * inch", "cBoreDepth" : "2.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "2.5312 * inch", "cBoreDiameter" : "3.875 * inch", "cBoreDepth" : "2.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "2 3/4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "2.7656 * inch", "cBoreDiameter" : "4.25 * inch", "cBoreDepth" : "2.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "2.7969 * inch", "cBoreDiameter" : "4.25 * inch", "cBoreDepth" : "2.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "2.7812 * inch", "cBoreDiameter" : "4.25 * inch", "cBoreDepth" : "2.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "3" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "3.0156 * inch", "cBoreDiameter" : "4.625 * inch", "cBoreDepth" : "3.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "3.0469 * inch", "cBoreDiameter" : "4.625 * inch", "cBoreDepth" : "3.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "3.0312 * inch", "cBoreDiameter" : "4.625 * inch", "cBoreDepth" : "3.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "3 1/4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "3.2656 * inch", "cBoreDiameter" : "5.0 * inch", "cBoreDepth" : "3.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "3.2969 * inch", "cBoreDiameter" : "5.0 * inch", "cBoreDepth" : "3.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "3.2812 * inch", "cBoreDiameter" : "5.0 * inch", "cBoreDepth" : "3.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "3 1/2" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "3.5156 * inch", "cBoreDiameter" : "5.375 * inch", "cBoreDepth" : "3.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "3.5469 * inch", "cBoreDiameter" : "5.375 * inch", "cBoreDepth" : "3.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "3.5312 * inch", "cBoreDiameter" : "5.375 * inch", "cBoreDepth" : "3.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "3 3/4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "3.7656 * inch", "cBoreDiameter" : "5.75 * inch", "cBoreDepth" : "3.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "3.7969 * inch", "cBoreDiameter" : "5.75 * inch", "cBoreDepth" : "3.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "3.7812 * inch", "cBoreDiameter" : "5.75 * inch", "cBoreDepth" : "3.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"}
            }
        },
        "4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"holeDiameter" : "4.0156 * inch", "cBoreDiameter" : "6.125 * inch", "cBoreDepth" : "4.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"},
                "Loose" : {"holeDiameter" : "4.0469 * inch", "cBoreDiameter" : "6.125 * inch", "cBoreDepth" : "4.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"},
                "Normal" : {"holeDiameter" : "4.0312 * inch", "cBoreDiameter" : "6.125 * inch", "cBoreDepth" : "4.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree"}
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
                        "50%" : {"holeDiameter" : "0.052 * inch", "tapDrillDiameter" : "0.052 * inch", "cBoreDiameter" : "1/8 * inch", "cBoreDepth" : "0.06 * inch", "cSinkDiameter" : "0.138 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0600 * inch"},
                        "75%" : {"holeDiameter" : "0.0469 * inch", "tapDrillDiameter" : "0.0469 * inch", "cBoreDiameter" : "1/8 * inch", "cBoreDepth" : "0.06 * inch", "cSinkDiameter" : "0.138 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0600 * inch"}
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
                        "50%" : {"holeDiameter" : "0.0625 * inch", "tapDrillDiameter" : "0.0625 * inch", "cBoreDiameter" : "5/32 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0730 * inch"},
                        "75%" : {"holeDiameter" : "0.0595 * inch", "tapDrillDiameter" : "0.0595 * inch", "cBoreDiameter" : "5/32 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0730 * inch"}
                    }
                },
                "72 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.0635 * inch", "tapDrillDiameter" : "0.0635 * inch", "cBoreDiameter" : "5/33 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0730 * inch"},
                        "75%" : {"holeDiameter" : "0.0595 * inch", "tapDrillDiameter" : "0.0595 * inch", "cBoreDiameter" : "5/33 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0730 * inch"}
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
                        "50%" : {"holeDiameter" : "0.073 * inch", "tapDrillDiameter" : "0.073 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0860 * inch"},
                        "75%" : {"holeDiameter" : "0.07 * inch", "tapDrillDiameter" : "0.07 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0860 * inch"}
                    }
                },
                "64 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.076 * inch", "tapDrillDiameter" : "0.076 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0860 * inch"},
                        "75%" : {"holeDiameter" : "0.07 * inch", "tapDrillDiameter" : "0.07 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0860 * inch"}
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
                        "50%" : {"holeDiameter" : "0.086 * inch", "tapDrillDiameter" : "0.086 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0990 * inch"},
                        "75%" : {"holeDiameter" : "0.0785 * inch", "tapDrillDiameter" : "0.0785 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0990 * inch"}
                    }
                },
                "56 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.089 * inch", "tapDrillDiameter" : "0.089 * inch", "cBoreDiameter" : "7/33 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0990 * inch"},
                        "75%" : {"holeDiameter" : "0.082 * inch", "tapDrillDiameter" : "0.082 * inch", "cBoreDiameter" : "7/33 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0990 * inch"}
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
                        "50%" : {"holeDiameter" : "0.096 * inch", "tapDrillDiameter" : "0.096 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1120 * inch"},
                        "75%" : {"holeDiameter" : "0.089 * inch", "tapDrillDiameter" : "0.089 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1120 * inch"}
                    }
                },
                "48 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.098 * inch", "tapDrillDiameter" : "0.098 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1120 * inch"},
                        "75%" : {"holeDiameter" : "0.0935 * inch", "tapDrillDiameter" : "0.0935 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1120 * inch"}
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
                        "50%" : {"holeDiameter" : "0.1094 * inch", "tapDrillDiameter" : "0.1094 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1250 * inch"},
                        "75%" : {"holeDiameter" : "0.1015 * inch", "tapDrillDiameter" : "0.1015 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1250 * inch"}
                    }
                },
                "44 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.11 * inch", "tapDrillDiameter" : "0.11 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1250 * inch"},
                        "75%" : {"holeDiameter" : "0.104 * inch", "tapDrillDiameter" : "0.104 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1250 * inch"}
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
                        "50%" : {"holeDiameter" : "0.116 * inch", "tapDrillDiameter" : "0.116 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1380 * inch"},
                        "75%" : {"holeDiameter" : "0.1065 * inch", "tapDrillDiameter" : "0.1065 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1380 * inch"}
                    }
                },
                "40 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.12 * inch", "tapDrillDiameter" : "0.12 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1380 * inch"},
                        "75%" : {"holeDiameter" : "0.113 * inch", "tapDrillDiameter" : "0.113 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1380 * inch"}
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
                        "50%" : {"holeDiameter" : "0.144 * inch", "tapDrillDiameter" : "0.144 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1640 * inch"},
                        "75%" : {"holeDiameter" : "0.136 * inch", "tapDrillDiameter" : "0.136 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1640 * inch"}
                    }
                },
                "36 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.147 * inch", "tapDrillDiameter" : "0.147 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1640 * inch"},
                        "75%" : {"holeDiameter" : "0.136 * inch", "tapDrillDiameter" : "0.136 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1640 * inch"}
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
                        "50%" : {"holeDiameter" : "0.161 * inch", "tapDrillDiameter" : "0.161 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1900 * inch"},
                        "75%" : {"holeDiameter" : "0.1495 * inch", "tapDrillDiameter" : "0.1495 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1900 * inch"}
                    }
                },
                "32 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.1695 * inch", "tapDrillDiameter" : "0.1695 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1900 * inch"},
                        "75%" : {"holeDiameter" : "0.159 * inch", "tapDrillDiameter" : "0.159 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1900 * inch"}
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
                        "50%" : {"holeDiameter" : "0.189 * inch", "tapDrillDiameter" : "0.189 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2160 * inch"},
                        "75%" : {"holeDiameter" : "0.177 * inch", "tapDrillDiameter" : "0.177 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2160 * inch"}
                    }
                },
                "28 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.1935 * inch", "tapDrillDiameter" : "0.1935 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2160 * inch"},
                        "75%" : {"holeDiameter" : "0.182 * inch", "tapDrillDiameter" : "0.182 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2160 * inch"}
                    }
                },
                "32 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.196 * inch", "tapDrillDiameter" : "0.196 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2160 * inch"},
                        "75%" : {"holeDiameter" : "0.185 * inch", "tapDrillDiameter" : "0.185 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2160 * inch"}
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
                        "50%" : {"holeDiameter" : "0.2188 * inch", "tapDrillDiameter" : "0.2188 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2500 * inch"},
                        "75%" : {"holeDiameter" : "0.201 * inch", "tapDrillDiameter" : "0.201 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2500 * inch"}
                    }
                },
                "28 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.228 * inch", "tapDrillDiameter" : "0.228 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2500 * inch"},
                        "75%" : {"holeDiameter" : "0.213 * inch", "tapDrillDiameter" : "0.213 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2500 * inch"}
                    }
                },
                "32 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.228 * inch", "tapDrillDiameter" : "0.228 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2500 * inch"},
                        "75%" : {"holeDiameter" : "0.2188 * inch", "tapDrillDiameter" : "0.2188 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2500 * inch"}
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
                        "50%" : {"holeDiameter" : "0.277 * inch", "tapDrillDiameter" : "0.277 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3125 * inch"},
                        "75%" : {"holeDiameter" : "0.257 * inch", "tapDrillDiameter" : "0.257 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3125 * inch"}
                    }
                },
                "24 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.2812 * inch", "tapDrillDiameter" : "0.2812 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3125 * inch"},
                        "75%" : {"holeDiameter" : "0.272 * inch", "tapDrillDiameter" : "0.272 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3125 * inch"}
                    }
                },
                "32 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.29 * inch", "tapDrillDiameter" : "0.29 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3125 * inch"},
                        "75%" : {"holeDiameter" : "0.2812 * inch", "tapDrillDiameter" : "0.2812 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3125 * inch"}
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
                        "50%" : {"holeDiameter" : "0.332 * inch", "tapDrillDiameter" : "0.332 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3750 * inch"},
                        "75%" : {"holeDiameter" : "0.3125 * inch", "tapDrillDiameter" : "0.3125 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3750 * inch"}
                    }
                },
                "24 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.348 * inch", "tapDrillDiameter" : "0.348 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3750 * inch"},
                        "75%" : {"holeDiameter" : "0.332 * inch", "tapDrillDiameter" : "0.332 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3750 * inch"}
                    }
                },
                "32 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.358 * inch", "tapDrillDiameter" : "0.358 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3750 * inch"},
                        "75%" : {"holeDiameter" : "0.3438 * inch", "tapDrillDiameter" : "0.3438 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3750 * inch"}
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
                        "50%" : {"holeDiameter" : "0.3906 * inch", "tapDrillDiameter" : "0.3906 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.4375 * inch"},
                        "75%" : {"holeDiameter" : "0.368 * inch", "tapDrillDiameter" : "0.368 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.4375 * inch"}
                    }
                },
                "20 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.4062 * inch", "tapDrillDiameter" : "0.4062 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.4375 * inch"},
                        "75%" : {"holeDiameter" : "0.3906 * inch", "tapDrillDiameter" : "0.3906 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.4375 * inch"}
                    }
                },
                "28 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.413 * inch", "tapDrillDiameter" : "0.413 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.4375 * inch"},
                        "75%" : {"holeDiameter" : "0.404 * inch", "tapDrillDiameter" : "0.404 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.4375 * inch"}
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
                        "50%" : {"holeDiameter" : "0.4531 * inch", "tapDrillDiameter" : "0.4531 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5000 * inch"},
                        "75%" : {"holeDiameter" : "0.4219 * inch", "tapDrillDiameter" : "0.4219 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5000 * inch"}
                    }
                },
                "20 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.4688 * inch", "tapDrillDiameter" : "0.4688 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5000 * inch"},
                        "75%" : {"holeDiameter" : "0.4531 * inch", "tapDrillDiameter" : "0.4531 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5000 * inch"}
                    }
                },
                "28 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.4688 * inch", "tapDrillDiameter" : "0.4688 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5000 * inch"},
                        "75%" : {"holeDiameter" : "0.4688 * inch", "tapDrillDiameter" : "0.4688 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5000 * inch"}
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
                        "50%" : {"holeDiameter" : "0.5156 * inch", "tapDrillDiameter" : "0.5156 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5625 * inch"},
                        "75%" : {"holeDiameter" : "0.4844 * inch", "tapDrillDiameter" : "0.4844 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5625 * inch"}
                    }
                },
                "18 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.5312 * inch", "tapDrillDiameter" : "0.5312 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5625 * inch"},
                        "75%" : {"holeDiameter" : "0.5156 * inch", "tapDrillDiameter" : "0.5156 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5625 * inch"}
                    }
                },
                "24 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.5312 * inch", "tapDrillDiameter" : "0.5312 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5625 * inch"},
                        "75%" : {"holeDiameter" : "0.5156 * inch", "tapDrillDiameter" : "0.5156 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5625 * inch"}
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
                        "50%" : {"holeDiameter" : "0.5625 * inch", "tapDrillDiameter" : "0.5625 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.6250 * inch"},
                        "75%" : {"holeDiameter" : "0.5312 * inch", "tapDrillDiameter" : "0.5312 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.6250 * inch"}
                    }
                },
                "18 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.5938 * inch", "tapDrillDiameter" : "0.5938 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.6250 * inch"},
                        "75%" : {"holeDiameter" : "0.5781 * inch", "tapDrillDiameter" : "0.5781 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.6250 * inch"}
                    }
                },
                "24 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.5938 * inch", "tapDrillDiameter" : "0.5938 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.6250 * inch"},
                        "75%" : {"holeDiameter" : "0.5781 * inch", "tapDrillDiameter" : "0.5781 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.6250 * inch"}
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
                        "50%" : {"holeDiameter" : "0.6875 * inch", "tapDrillDiameter" : "0.6875 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.7500 * inch"},
                        "75%" : {"holeDiameter" : "0.6562 * inch", "tapDrillDiameter" : "0.6562 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.7500 * inch"}
                    }
                },
                "16 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.7031 * inch", "tapDrillDiameter" : "0.7031 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.7500 * inch"},
                        "75%" : {"holeDiameter" : "0.6875 * inch", "tapDrillDiameter" : "0.6875 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.7500 * inch"}
                    }
                },
                "20 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.7188 * inch", "tapDrillDiameter" : "0.7188 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.7500 * inch"},
                        "75%" : {"holeDiameter" : "0.7031 * inch", "tapDrillDiameter" : "0.7031 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.7500 * inch"}
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
                        "50%" : {"holeDiameter" : "0.7969 * inch", "tapDrillDiameter" : "0.7969 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.8750 * inch"},
                        "75%" : {"holeDiameter" : "0.7656 * inch", "tapDrillDiameter" : "0.7656 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.8750 * inch"}
                    }
                },
                "14 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.8281 * inch", "tapDrillDiameter" : "0.8281 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.8750 * inch"},
                        "75%" : {"holeDiameter" : "0.8125 * inch", "tapDrillDiameter" : "0.8125 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.8750 * inch"}
                    }
                },
                "20 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.8438 * inch", "tapDrillDiameter" : "0.8438 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.8750 * inch"},
                        "75%" : {"holeDiameter" : "0.8281 * inch", "tapDrillDiameter" : "0.8281 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.8750 * inch"}
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
                        "50%" : {"holeDiameter" : "0.9219 * inch", "tapDrillDiameter" : "0.9219 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.0000 * inch"},
                        "75%" : {"holeDiameter" : "0.875 * inch", "tapDrillDiameter" : "0.875 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.0000 * inch"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.9531 * inch", "tapDrillDiameter" : "0.9531 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.0000 * inch"},
                        "75%" : {"holeDiameter" : "0.9219 * inch", "tapDrillDiameter" : "0.9219 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.0000 * inch"}
                    }
                },
                "20 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.9688 * inch", "tapDrillDiameter" : "0.9688 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.0000 * inch"},
                        "75%" : {"holeDiameter" : "0.9531 * inch", "tapDrillDiameter" : "0.9531 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.0000 * inch"}
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
                        "50%" : {"holeDiameter" : "1.0313 * inch", "tapDrillDiameter" : "1.0313 * inch", "cBoreDiameter" : "1.8125 * inch", "cBoreDepth" : "1.125 * inch", "cSinkDiameter" : "2.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.125 * inch"},
                        "75%" : {"holeDiameter" : "0.9844 * inch", "tapDrillDiameter" : "0.9844 * inch", "cBoreDiameter" : "1.8125 * inch", "cBoreDepth" : "1.125 * inch", "cSinkDiameter" : "2.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.125 * inch"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.0781 * inch", "tapDrillDiameter" : "1.0781 * inch", "cBoreDiameter" : "1.8125 * inch", "cBoreDepth" : "1.125 * inch", "cSinkDiameter" : "2.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.125 * inch"},
                        "75%" : {"holeDiameter" : "1.0469 * inch", "tapDrillDiameter" : "1.0469 * inch", "cBoreDiameter" : "1.8125 * inch", "cBoreDepth" : "1.125 * inch", "cSinkDiameter" : "2.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.125 * inch"}
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
                        "50%" : {"holeDiameter" : "1.1562 * inch", "tapDrillDiameter" : "1.1562 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.2500 * inch"},
                        "75%" : {"holeDiameter" : "1.1094 * inch", "tapDrillDiameter" : "1.1094 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.2500 * inch"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.2031 * inch", "tapDrillDiameter" : "1.2031 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.2500 * inch"},
                        "75%" : {"holeDiameter" : "1.1719 * inch", "tapDrillDiameter" : "1.1719 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.2500 * inch"}
                    }
                },
                "18 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.2187 * inch", "tapDrillDiameter" : "1.2187 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.2500 * inch"},
                        "75%" : {"holeDiameter" : "1.1875 * inch", "tapDrillDiameter" : "1.1875 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.2500 * inch"}
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
                        "50%" : {"holeDiameter" : "1.2656 * inch", "tapDrillDiameter" : "1.2656 * inch", "cBoreDiameter" : "2.1875 * inch", "cBoreDepth" : "1.375 * inch", "cSinkDiameter" : "2.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.375 * inch"},
                        "75%" : {"holeDiameter" : "1.2187 * inch", "tapDrillDiameter" : "1.2187 * inch", "cBoreDiameter" : "2.1875 * inch", "cBoreDepth" : "1.375 * inch", "cSinkDiameter" : "2.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.375 * inch"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.3281 * inch", "tapDrillDiameter" : "1.3281 * inch", "cBoreDiameter" : "2.1875 * inch", "cBoreDepth" : "1.375 * inch", "cSinkDiameter" : "2.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.375 * inch"},
                        "75%" : {"holeDiameter" : "1.2969 * inch", "tapDrillDiameter" : "1.2969 * inch", "cBoreDiameter" : "2.1875 * inch", "cBoreDepth" : "1.375 * inch", "cSinkDiameter" : "2.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.375 * inch"}
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
                        "50%" : {"holeDiameter" : "1.3906 * inch", "tapDrillDiameter" : "1.3906 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.5000 * inch"},
                        "75%" : {"holeDiameter" : "1.3437 * inch", "tapDrillDiameter" : "1.3437 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.5000 * inch"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.4375 * inch", "tapDrillDiameter" : "1.4375 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.5000 * inch"},
                        "75%" : {"holeDiameter" : "1.4219 * inch", "tapDrillDiameter" : "1.4219 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.5000 * inch"}
                    }
                },
                "18 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.4687 * inch", "tapDrillDiameter" : "1.4687 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.5000 * inch"},
                        "75%" : {"holeDiameter" : "1.4375 * inch", "tapDrillDiameter" : "1.4375 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.5000 * inch"}
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
                        "50%" : {"holeDiameter" : "1.6417 * inch", "tapDrillDiameter" : "1.6417 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.7500 * inch"},
                        "75%" : {"holeDiameter" : "1.5625 * inch", "tapDrillDiameter" : "1.5625 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.7500 * inch"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.6959 * inch", "tapDrillDiameter" : "1.6959 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.7500 * inch"},
                        "75%" : {"holeDiameter" : "1.6735 * inch", "tapDrillDiameter" : "1.6735 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.7500 * inch"}
                    }
                },
                "16 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.7094 * inch", "tapDrillDiameter" : "1.7094 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.7500 * inch"},
                        "75%" : {"holeDiameter" : "1.6925 * inch", "tapDrillDiameter" : "1.6925 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.7500 * inch"}
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
                        "50%" : {"holeDiameter" : "1.8594 * inch", "tapDrillDiameter" : "1.8594 * inch", "cBoreDiameter" : "3.125 * inch", "cBoreDepth" : "2.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.0 * inch"},
                        "75%" : {"holeDiameter" : "1.7812 * inch", "tapDrillDiameter" : "1.7812 * inch", "cBoreDiameter" : "3.125 * inch", "cBoreDepth" : "2.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.0 * inch"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.9531 * inch", "tapDrillDiameter" : "1.9531 * inch", "cBoreDiameter" : "3.125 * inch", "cBoreDepth" : "2.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.0 * inch"},
                        "75%" : {"holeDiameter" : "1.9219 * inch", "tapDrillDiameter" : "1.9219 * inch", "cBoreDiameter" : "3.125 * inch", "cBoreDepth" : "2.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.0 * inch"}
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
                        "50%" : {"holeDiameter" : "2.1057 * inch", "tapDrillDiameter" : "2.1057 * inch", "cBoreDiameter" : "3.5 * inch", "cBoreDepth" : "2.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.25 * inch"},
                        "75%" : {"holeDiameter" : "2.036 * inch", "tapDrillDiameter" : "2.036 * inch", "cBoreDiameter" : "3.5 * inch", "cBoreDepth" : "2.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.25 * inch"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "2.1959 * inch", "tapDrillDiameter" : "2.1959 * inch", "cBoreDiameter" : "3.5 * inch", "cBoreDepth" : "2.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.25 * inch"},
                        "75%" : {"holeDiameter" : "2.1735 * inch", "tapDrillDiameter" : "2.1735 * inch", "cBoreDiameter" : "3.5 * inch", "cBoreDepth" : "2.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.25 * inch"}
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
                        "50%" : {"holeDiameter" : "2.3376 * inch", "tapDrillDiameter" : "2.3376 * inch", "cBoreDiameter" : "3.875 * inch", "cBoreDepth" : "2.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.5 * inch"},
                        "75%" : {"holeDiameter" : "2.2575 * inch", "tapDrillDiameter" : "2.2575 * inch", "cBoreDiameter" : "3.875 * inch", "cBoreDepth" : "2.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.5 * inch"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "2.4459 * inch", "tapDrillDiameter" : "2.4459 * inch", "cBoreDiameter" : "3.875 * inch", "cBoreDepth" : "2.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.5 * inch"},
                        "75%" : {"holeDiameter" : "2.435 * inch", "tapDrillDiameter" : "2.435 * inch", "cBoreDiameter" : "3.875 * inch", "cBoreDepth" : "2.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.5 * inch"}
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
                        "50%" : {"holeDiameter" : "2.5876 * inch", "tapDrillDiameter" : "2.5876 * inch", "cBoreDiameter" : "4.25 * inch", "cBoreDepth" : "2.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.75 * inch"},
                        "75%" : {"holeDiameter" : "2.5075 * inch", "tapDrillDiameter" : "2.5075 * inch", "cBoreDiameter" : "4.25 * inch", "cBoreDepth" : "2.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.75 * inch"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "2.6959 * inch", "tapDrillDiameter" : "2.6959 * inch", "cBoreDiameter" : "4.25 * inch", "cBoreDepth" : "2.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.75 * inch"},
                        "75%" : {"holeDiameter" : "2.6735 * inch", "tapDrillDiameter" : "2.6735 * inch", "cBoreDiameter" : "4.25 * inch", "cBoreDepth" : "2.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.75 * inch"}
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
                        "50%" : {"holeDiameter" : "2.8376 * inch", "tapDrillDiameter" : "2.8376 * inch", "cBoreDiameter" : "4.625 * inch", "cBoreDepth" : "3.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.0 * inch"},
                        "75%" : {"holeDiameter" : "2.7575 * inch", "tapDrillDiameter" : "2.7575 * inch", "cBoreDiameter" : "4.625 * inch", "cBoreDepth" : "3.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.0 * inch"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "2.9459 * inch", "tapDrillDiameter" : "2.9459 * inch", "cBoreDiameter" : "4.625 * inch", "cBoreDepth" : "3.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.0 * inch"},
                        "75%" : {"holeDiameter" : "2.9235 * inch", "tapDrillDiameter" : "2.9235 * inch", "cBoreDiameter" : "4.625 * inch", "cBoreDepth" : "3.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.0 * inch"}
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
                        "50%" : {"holeDiameter" : "3.0876 * inch", "tapDrillDiameter" : "3.0876 * inch", "cBoreDiameter" : "5.0 * inch", "cBoreDepth" : "3.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.25 * inch"},
                        "75%" : {"holeDiameter" : "3.0075 * inch", "tapDrillDiameter" : "3.0075 * inch", "cBoreDiameter" : "5.0 * inch", "cBoreDepth" : "3.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.25 * inch"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "3.1959 * inch", "tapDrillDiameter" : "3.1959 * inch", "cBoreDiameter" : "5.0 * inch", "cBoreDepth" : "3.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.25 * inch"},
                        "75%" : {"holeDiameter" : "3.1735 * inch", "tapDrillDiameter" : "3.1735 * inch", "cBoreDiameter" : "5.0 * inch", "cBoreDepth" : "3.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.25 * inch"}
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
                        "50%" : {"holeDiameter" : "3.3376 * inch", "tapDrillDiameter" : "3.3376 * inch", "cBoreDiameter" : "5.375 * inch", "cBoreDepth" : "3.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.5 * inch"},
                        "75%" : {"holeDiameter" : "3.2575 * inch", "tapDrillDiameter" : "3.2575 * inch", "cBoreDiameter" : "5.375 * inch", "cBoreDepth" : "3.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.5 * inch"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "3.4459 * inch", "tapDrillDiameter" : "3.4459 * inch", "cBoreDiameter" : "5.375 * inch", "cBoreDepth" : "3.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.5 * inch"},
                        "75%" : {"holeDiameter" : "3.4235 * inch", "tapDrillDiameter" : "3.4235 * inch", "cBoreDiameter" : "5.375 * inch", "cBoreDepth" : "3.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.5 * inch"}
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
                        "50%" : {"holeDiameter" : "3.5876 * inch", "tapDrillDiameter" : "3.5876 * inch", "cBoreDiameter" : "5.75 * inch", "cBoreDepth" : "3.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.75 * inch"},
                        "75%" : {"holeDiameter" : "3.5075 * inch", "tapDrillDiameter" : "3.5075 * inch", "cBoreDiameter" : "5.75 * inch", "cBoreDepth" : "3.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.75 * inch"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "3.6959 * inch", "tapDrillDiameter" : "3.6959 * inch", "cBoreDiameter" : "5.75 * inch", "cBoreDepth" : "3.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.75 * inch"},
                        "75%" : {"holeDiameter" : "3.6735 * inch", "tapDrillDiameter" : "3.6735 * inch", "cBoreDiameter" : "5.75 * inch", "cBoreDepth" : "3.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.75 * inch"}
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
                        "50%" : {"holeDiameter" : "3.8376 * inch", "tapDrillDiameter" : "3.8376 * inch", "cBoreDiameter" : "6.125 * inch", "cBoreDepth" : "4.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "4.0 * inch"},
                        "75%" : {"holeDiameter" : "3.7575 * inch", "tapDrillDiameter" : "3.7575 * inch", "cBoreDiameter" : "6.125 * inch", "cBoreDepth" : "4.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "4.0 * inch"}
                    }
                },
                "12 tpi" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "3.9459 * inch", "tapDrillDiameter" : "3.9459 * inch", "cBoreDiameter" : "6.125 * inch", "cBoreDepth" : "4.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "4.0 * inch"},
                        "75%" : {"holeDiameter" : "3.9235 * inch", "tapDrillDiameter" : "3.9235 * inch", "cBoreDiameter" : "6.125 * inch", "cBoreDepth" : "4.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "4.0 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.052 * inch", "holeDiameter" : "0.067 * inch", "cBoreDiameter" : "1/8 * inch", "cBoreDepth" : "0.06 * inch", "cSinkDiameter" : "0.138 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0600 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.0469 * inch", "holeDiameter" : "0.067 * inch", "cBoreDiameter" : "1/8 * inch", "cBoreDepth" : "0.06 * inch", "cSinkDiameter" : "0.138 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0600 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.052 * inch", "holeDiameter" : "0.094 * inch", "cBoreDiameter" : "1/8 * inch", "cBoreDepth" : "0.06 * inch", "cSinkDiameter" : "0.138 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0600 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.0469 * inch", "holeDiameter" : "0.094 * inch", "cBoreDiameter" : "1/8 * inch", "cBoreDepth" : "0.06 * inch", "cSinkDiameter" : "0.138 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0600 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.052 * inch", "holeDiameter" : "0.076 * inch", "cBoreDiameter" : "1/8 * inch", "cBoreDepth" : "0.06 * inch", "cSinkDiameter" : "0.138 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0600 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.0469 * inch", "holeDiameter" : "0.076 * inch", "cBoreDiameter" : "1/8 * inch", "cBoreDepth" : "0.06 * inch", "cSinkDiameter" : "0.138 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0600 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.0625 * inch", "holeDiameter" : "0.081 * inch", "cBoreDiameter" : "5/32 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0730 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.0595 * inch", "holeDiameter" : "0.081 * inch", "cBoreDiameter" : "5/32 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0730 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.0625 * inch", "holeDiameter" : "0.104 * inch", "cBoreDiameter" : "5/32 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0730 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.0595 * inch", "holeDiameter" : "0.104 * inch", "cBoreDiameter" : "5/32 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0730 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.0625 * inch", "holeDiameter" : "0.089 * inch", "cBoreDiameter" : "5/32 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0730 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.0595 * inch", "holeDiameter" : "0.089 * inch", "cBoreDiameter" : "5/32 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0730 * inch"}
                            }
                        }
                    }
                },
                "72 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.0635 * inch", "holeDiameter" : "0.081 * inch", "cBoreDiameter" : "5/33 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0730 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.0595 * inch", "holeDiameter" : "0.081 * inch", "cBoreDiameter" : "5/33 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0730 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.0635 * inch", "holeDiameter" : "0.104 * inch", "cBoreDiameter" : "5/33 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0730 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.0595 * inch", "holeDiameter" : "0.104 * inch", "cBoreDiameter" : "5/33 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0730 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.0635 * inch", "holeDiameter" : "0.089 * inch", "cBoreDiameter" : "5/33 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0730 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.0595 * inch", "holeDiameter" : "0.089 * inch", "cBoreDiameter" : "5/33 * inch", "cBoreDepth" : "0.073 * inch", "cSinkDiameter" : "0.168 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0730 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.073 * inch", "holeDiameter" : "0.094 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0860 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.07 * inch", "holeDiameter" : "0.094 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0860 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.073 * inch", "holeDiameter" : "0.116 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0860 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.07 * inch", "holeDiameter" : "0.116 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0860 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.073 * inch", "holeDiameter" : "0.102 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0860 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.07 * inch", "holeDiameter" : "0.102 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0860 * inch"}
                            }
                        }
                    }
                },
                "64 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.076 * inch", "holeDiameter" : "0.094 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0860 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.07 * inch", "holeDiameter" : "0.094 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0860 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.076 * inch", "holeDiameter" : "0.116 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0860 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.07 * inch", "holeDiameter" : "0.116 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0860 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.076 * inch", "holeDiameter" : "0.102 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0860 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.07 * inch", "holeDiameter" : "0.102 * inch", "cBoreDiameter" : "3/16 * inch", "cBoreDepth" : "0.086 * inch", "cSinkDiameter" : "0.197 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0860 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.086 * inch", "holeDiameter" : "0.106 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0990 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.0785 * inch", "holeDiameter" : "0.106 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0990 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.086 * inch", "holeDiameter" : "0.128 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0990 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.0785 * inch", "holeDiameter" : "0.128 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0990 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.086 * inch", "holeDiameter" : "0.116 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0990 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.0785 * inch", "holeDiameter" : "0.116 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0990 * inch"}
                            }
                        }
                    }
                },
                "56 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.089 * inch", "holeDiameter" : "0.106 * inch", "cBoreDiameter" : "7/33 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0990 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.082 * inch", "holeDiameter" : "0.106 * inch", "cBoreDiameter" : "7/33 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0990 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.089 * inch", "holeDiameter" : "0.128 * inch", "cBoreDiameter" : "7/33 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0990 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.082 * inch", "holeDiameter" : "0.128 * inch", "cBoreDiameter" : "7/33 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0990 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.089 * inch", "holeDiameter" : "0.116 * inch", "cBoreDiameter" : "7/33 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0990 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.082 * inch", "holeDiameter" : "0.116 * inch", "cBoreDiameter" : "7/33 * inch", "cBoreDepth" : "0.099 * inch", "cSinkDiameter" : "0.226 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.0990 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.096 * inch", "holeDiameter" : "0.120 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1120 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.089 * inch", "holeDiameter" : "0.120 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1120 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.096 * inch", "holeDiameter" : "0.144 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1120 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.089 * inch", "holeDiameter" : "0.144 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1120 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.096 * inch", "holeDiameter" : "0.128 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1120 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.089 * inch", "holeDiameter" : "0.128 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1120 * inch"}
                            }
                        }
                    }
                },
                "48 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.098 * inch", "holeDiameter" : "0.120 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1120 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.0935 * inch", "holeDiameter" : "0.120 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1120 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.098 * inch", "holeDiameter" : "0.144 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1120 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.0935 * inch", "holeDiameter" : "0.144 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1120 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.098 * inch", "holeDiameter" : "0.128 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1120 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.0935 * inch", "holeDiameter" : "0.128 * inch", "cBoreDiameter" : "7/32 * inch", "cBoreDepth" : "0.112 * inch", "cSinkDiameter" : "0.255 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1120 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.1094 * inch", "holeDiameter" : "0.141 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1250 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.1015 * inch", "holeDiameter" : "0.141 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1250 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.1094 * inch", "holeDiameter" : "0.172 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1250 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.1015 * inch", "holeDiameter" : "0.172 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1250 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.1094 * inch", "holeDiameter" : "0.156 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1250 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.1015 * inch", "holeDiameter" : "0.156 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1250 * inch"}
                            }
                        }
                    }
                },
                "44 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.11 * inch", "holeDiameter" : "0.141 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1250 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.104 * inch", "holeDiameter" : "0.141 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1250 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.11 * inch", "holeDiameter" : "0.172 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1250 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.104 * inch", "holeDiameter" : "0.172 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1250 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.11 * inch", "holeDiameter" : "0.156 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1250 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.104 * inch", "holeDiameter" : "0.156 * inch", "cBoreDiameter" : "1/4 * inch", "cBoreDepth" : "0.125 * inch", "cSinkDiameter" : "0.281 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1250 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.116 * inch", "holeDiameter" : "0.154 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1380 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.1065 * inch", "holeDiameter" : "0.154 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1380 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.116 * inch", "holeDiameter" : "0.185 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1380 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.1065 * inch", "holeDiameter" : "0.185 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1380 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.116 * inch", "holeDiameter" : "0.170 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1380 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.1065 * inch", "holeDiameter" : "0.170 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1380 * inch"}
                            }
                        }
                    }
                },
                "40 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.12 * inch", "holeDiameter" : "0.154 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1380 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.113 * inch", "holeDiameter" : "0.154 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1380 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.12 * inch", "holeDiameter" : "0.185 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1380 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.113 * inch", "holeDiameter" : "0.185 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1380 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.12 * inch", "holeDiameter" : "0.170 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1380 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.113 * inch", "holeDiameter" : "0.170 * inch", "cBoreDiameter" : "9/32 * inch", "cBoreDepth" : "0.138 * inch", "cSinkDiameter" : "0.307 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1380 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.144 * inch", "holeDiameter" : "0.180 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1640 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.136 * inch", "holeDiameter" : "0.180 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1640 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.144 * inch", "holeDiameter" : "0.213 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1640 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.136 * inch", "holeDiameter" : "0.213 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1640 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.144 * inch", "holeDiameter" : "0.196 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1640 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.136 * inch", "holeDiameter" : "0.196 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1640 * inch"}
                            }
                        }
                    }
                },
                "36 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.147 * inch", "holeDiameter" : "0.180 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1640 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.136 * inch", "holeDiameter" : "0.180 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1640 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.147 * inch", "holeDiameter" : "0.213 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1640 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.136 * inch", "holeDiameter" : "0.213 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1640 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.147 * inch", "holeDiameter" : "0.196 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1640 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.136 * inch", "holeDiameter" : "0.196 * inch", "cBoreDiameter" : "5/16 * inch", "cBoreDepth" : "0.164 * inch", "cSinkDiameter" : "0.359 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1640 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.161 * inch", "holeDiameter" : "0.206 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1900 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.1495 * inch", "holeDiameter" : "0.206 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1900 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.161 * inch", "holeDiameter" : "0.238 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1900 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.1495 * inch", "holeDiameter" : "0.238 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1900 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.161 * inch", "holeDiameter" : "0.221 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1900 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.1495 * inch", "holeDiameter" : "0.221 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1900 * inch"}
                            }
                        }
                    }
                },
                "32 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.1695 * inch", "holeDiameter" : "0.206 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1900 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.159 * inch", "holeDiameter" : "0.206 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1900 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.1695 * inch", "holeDiameter" : "0.238 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1900 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.159 * inch", "holeDiameter" : "0.238 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1900 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.1695 * inch", "holeDiameter" : "0.221 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1900 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.159 * inch", "holeDiameter" : "0.221 * inch", "cBoreDiameter" : "3/8 * inch", "cBoreDepth" : "0.19 * inch", "cSinkDiameter" : "0.411 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.1900 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.189 * inch", "holeDiameter" : "0.221 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2160 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.177 * inch", "holeDiameter" : "0.221 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2160 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.189 * inch", "holeDiameter" : "0.246 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2160 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.177 * inch", "holeDiameter" : "0.246 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2160 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.189 * inch", "holeDiameter" : "0.228 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2160 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.177 * inch", "holeDiameter" : "0.228 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2160 * inch"}
                            }
                        }
                    }
                },
                "28 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.1935 * inch", "holeDiameter" : "0.221 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2160 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.182 * inch", "holeDiameter" : "0.221 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2160 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.1935 * inch", "holeDiameter" : "0.246 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2160 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.182 * inch", "holeDiameter" : "0.246 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2160 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.1935 * inch", "holeDiameter" : "0.228 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2160 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.182 * inch", "holeDiameter" : "0.228 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2160 * inch"}
                            }
                        }
                    }
                },
                "32 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.196 * inch", "holeDiameter" : "0.221 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2160 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.185 * inch", "holeDiameter" : "0.221 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2160 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.196 * inch", "holeDiameter" : "0.246 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2160 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.185 * inch", "holeDiameter" : "0.246 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2160 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.196 * inch", "holeDiameter" : "0.228 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2160 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.185 * inch", "holeDiameter" : "0.228 * inch", "cBoreDiameter" : "13/32 * inch", "cBoreDepth" : "0.216 * inch", "cSinkDiameter" : "0.450 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2160 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.2188 * inch", "holeDiameter" : "0.266 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2500 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.201 * inch", "holeDiameter" : "0.266 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2500 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.2188 * inch", "holeDiameter" : "0.297 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2500 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.201 * inch", "holeDiameter" : "0.297 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2500 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.2188 * inch", "holeDiameter" : "0.281 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2500 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.201 * inch", "holeDiameter" : "0.281 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2500 * inch"}
                            }
                        }
                    }
                },
                "28 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.228 * inch", "holeDiameter" : "0.266 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2500 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.213 * inch", "holeDiameter" : "0.266 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2500 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.228 * inch", "holeDiameter" : "0.297 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2500 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.213 * inch", "holeDiameter" : "0.297 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2500 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.228 * inch", "holeDiameter" : "0.281 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2500 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.213 * inch", "holeDiameter" : "0.281 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2500 * inch"}
                            }
                        }
                    }
                },
                "32 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.228 * inch", "holeDiameter" : "0.266 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2500 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.2188 * inch", "holeDiameter" : "0.266 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2500 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.228 * inch", "holeDiameter" : "0.297 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2500 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.2188 * inch", "holeDiameter" : "0.297 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2500 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.228 * inch", "holeDiameter" : "0.281 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2500 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.2188 * inch", "holeDiameter" : "0.281 * inch", "cBoreDiameter" : "7/16 * inch", "cBoreDepth" : "0.25 * inch", "cSinkDiameter" : "0.531 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.2500 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.277 * inch", "holeDiameter" : "0.328 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3125 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.257 * inch", "holeDiameter" : "0.328 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3125 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.277 * inch", "holeDiameter" : "0.359 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3125 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.257 * inch", "holeDiameter" : "0.359 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3125 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.277 * inch", "holeDiameter" : "0.344 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3125 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.257 * inch", "holeDiameter" : "0.344 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3125 * inch"}
                            }
                        }
                    }
                },
                "24 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.2812 * inch", "holeDiameter" : "0.328 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3125 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.272 * inch", "holeDiameter" : "0.328 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3125 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.2812 * inch", "holeDiameter" : "0.359 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3125 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.272 * inch", "holeDiameter" : "0.359 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3125 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.2812 * inch", "holeDiameter" : "0.344 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3125 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.272 * inch", "holeDiameter" : "0.344 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3125 * inch"}
                            }
                        }
                    }
                },
                "32 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.29 * inch", "holeDiameter" : "0.328 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3125 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.2812 * inch", "holeDiameter" : "0.328 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3125 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.29 * inch", "holeDiameter" : "0.359 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3125 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.2812 * inch", "holeDiameter" : "0.359 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3125 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.29 * inch", "holeDiameter" : "0.344 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3125 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.2812 * inch", "holeDiameter" : "0.344 * inch", "cBoreDiameter" : "17/32 * inch", "cBoreDepth" : "0.3125 * inch", "cSinkDiameter" : "0.656 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3125 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.332 * inch", "holeDiameter" : "0.391 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3750 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.3125 * inch", "holeDiameter" : "0.391 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3750 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.332 * inch", "holeDiameter" : "0.422 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3750 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.3125 * inch", "holeDiameter" : "0.422 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3750 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.332 * inch", "holeDiameter" : "0.406 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3750 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.3125 * inch", "holeDiameter" : "0.406 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3750 * inch"}
                            }
                        }
                    }
                },
                "24 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.348 * inch", "holeDiameter" : "0.391 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3750 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.332 * inch", "holeDiameter" : "0.391 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3750 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.348 * inch", "holeDiameter" : "0.422 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3750 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.332 * inch", "holeDiameter" : "0.422 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3750 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.348 * inch", "holeDiameter" : "0.406 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3750 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.332 * inch", "holeDiameter" : "0.406 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3750 * inch"}
                            }
                        }
                    }
                },
                "32 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.358 * inch", "holeDiameter" : "0.391 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3750 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.3438 * inch", "holeDiameter" : "0.391 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3750 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.358 * inch", "holeDiameter" : "0.422 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3750 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.3438 * inch", "holeDiameter" : "0.422 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3750 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.358 * inch", "holeDiameter" : "0.406 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3750 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.3438 * inch", "holeDiameter" : "0.406 * inch", "cBoreDiameter" : "5/8 * inch", "cBoreDepth" : "0.375 * inch", "cSinkDiameter" : "0.781 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.3750 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.3906 * inch", "holeDiameter" : "0.453 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.4375 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.368 * inch", "holeDiameter" : "0.453 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.4375 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.3906 * inch", "holeDiameter" : "0.484 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.4375 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.368 * inch", "holeDiameter" : "0.484 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.4375 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.3906 * inch", "holeDiameter" : "0.469 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.4375 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.368 * inch", "holeDiameter" : "0.469 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.4375 * inch"}
                            }
                        }
                    }
                },
                "20 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4062 * inch", "holeDiameter" : "0.453 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.4375 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.3906 * inch", "holeDiameter" : "0.453 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.4375 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4062 * inch", "holeDiameter" : "0.484 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.4375 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.3906 * inch", "holeDiameter" : "0.484 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.4375 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4062 * inch", "holeDiameter" : "0.469 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.4375 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.3906 * inch", "holeDiameter" : "0.469 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.4375 * inch"}
                            }
                        }
                    }
                },
                "28 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.413 * inch", "holeDiameter" : "0.453 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.4375 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.404 * inch", "holeDiameter" : "0.453 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.4375 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.413 * inch", "holeDiameter" : "0.484 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.4375 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.404 * inch", "holeDiameter" : "0.484 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.4375 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.413 * inch", "holeDiameter" : "0.4687 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.4375 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.404 * inch", "holeDiameter" : "0.4687 * inch", "cBoreDiameter" : "23/32 * inch", "cBoreDepth" : "0.4375 * inch", "cSinkDiameter" : "0.844 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.4375 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4531 * inch", "holeDiameter" : "0.531 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5000 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.4219 * inch", "holeDiameter" : "0.531 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5000 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4531 * inch", "holeDiameter" : "0.609 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5000 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.4219 * inch", "holeDiameter" : "0.609 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5000 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4531 * inch", "holeDiameter" : "0.562 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5000 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.4219 * inch", "holeDiameter" : "0.562 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5000 * inch"}
                            }
                        }
                    }
                },
                "20 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4688 * inch", "holeDiameter" : "0.531 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5000 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.4531 * inch", "holeDiameter" : "0.531 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5000 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4688 * inch", "holeDiameter" : "0.609 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5000 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.4531 * inch", "holeDiameter" : "0.609 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5000 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4688 * inch", "holeDiameter" : "0.562 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5000 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.4531 * inch", "holeDiameter" : "0.562 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5000 * inch"}
                            }
                        }
                    }
                },
                "28 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4688 * inch", "holeDiameter" : "0.531 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5000 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.4688 * inch", "holeDiameter" : "0.531 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5000 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4688 * inch", "holeDiameter" : "0.609 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5000 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.4688 * inch", "holeDiameter" : "0.609 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5000 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.4688 * inch", "holeDiameter" : "0.562 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5000 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.4688 * inch", "holeDiameter" : "0.562 * inch", "cBoreDiameter" : "13/16 * inch", "cBoreDepth" : "0.5 * inch", "cSinkDiameter" : "0.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5000 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5156 * inch", "holeDiameter" : "0.5781 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5625 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.4844 * inch", "holeDiameter" : "0.5781 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5625 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5156 * inch", "holeDiameter" : "0.6094 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5625 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.4844 * inch", "holeDiameter" : "0.6094 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5625 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5156 * inch", "holeDiameter" : "0.5938 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5625 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.4844 * inch", "holeDiameter" : "0.5938 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5625 * inch"}
                            }
                        }
                    }
                },
                "18 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5312 * inch", "holeDiameter" : "0.5781 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5625 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.5156 * inch", "holeDiameter" : "0.5781 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5625 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5312 * inch", "holeDiameter" : "0.6094 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5625 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.5156 * inch", "holeDiameter" : "0.6094 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5625 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5312 * inch", "holeDiameter" : "0.5938 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5625 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.5156 * inch", "holeDiameter" : "0.5938 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5625 * inch"}
                            }
                        }
                    }
                },
                "24 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5312 * inch", "holeDiameter" : "0.5781 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5625 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.5156 * inch", "holeDiameter" : "0.5781 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5625 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5312 * inch", "holeDiameter" : "0.6094 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5625 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.5156 * inch", "holeDiameter" : "0.6094 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5625 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5312 * inch", "holeDiameter" : "0.5938 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5625 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.5156 * inch", "holeDiameter" : "0.5938 * inch", "cBoreDiameter" : "29/32 * inch", "cBoreDepth" : "0.5625 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.5625 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5625 * inch", "holeDiameter" : "0.656 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.6250 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.5312 * inch", "holeDiameter" : "0.656 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.6250 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5625 * inch", "holeDiameter" : "0.734 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.6250 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.5312 * inch", "holeDiameter" : "0.734 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.6250 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5625 * inch", "holeDiameter" : "0.688 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.6250 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.5312 * inch", "holeDiameter" : "0.688 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.6250 * inch"}
                            }
                        }
                    }
                },
                "18 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5938 * inch", "holeDiameter" : "0.656 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.6250 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.5781 * inch", "holeDiameter" : "0.656 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.6250 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5938 * inch", "holeDiameter" : "0.734 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.6250 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.5781 * inch", "holeDiameter" : "0.734 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.6250 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5938 * inch", "holeDiameter" : "0.688 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.6250 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.5781 * inch", "holeDiameter" : "0.688 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.6250 * inch"}
                            }
                        }
                    }
                },
                "24 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5938 * inch", "holeDiameter" : "0.656 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.6250 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.5781 * inch", "holeDiameter" : "0.656 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.6250 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5938 * inch", "holeDiameter" : "0.734 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.6250 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.5781 * inch", "holeDiameter" : "0.734 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.6250 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.5938 * inch", "holeDiameter" : "0.688 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.6250 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.5781 * inch", "holeDiameter" : "0.688 * inch", "cBoreDiameter" : "1 * inch", "cBoreDepth" : "0.625 * inch", "cSinkDiameter" : "1.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.6250 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.6875 * inch", "holeDiameter" : "0.781 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.7500 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.6562 * inch", "holeDiameter" : "0.781 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.7500 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.6875 * inch", "holeDiameter" : "0.906 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.7500 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.6562 * inch", "holeDiameter" : "0.906 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.7500 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.6875 * inch", "holeDiameter" : "0.812 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.7500 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.6562 * inch", "holeDiameter" : "0.812 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.7500 * inch"}
                            }
                        }
                    }
                },
                "16 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7031 * inch", "holeDiameter" : "0.781 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.7500 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.6875 * inch", "holeDiameter" : "0.781 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.7500 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7031 * inch", "holeDiameter" : "0.906 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.7500 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.6875 * inch", "holeDiameter" : "0.906 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.7500 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7031 * inch", "holeDiameter" : "0.812 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.7500 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.6875 * inch", "holeDiameter" : "0.812 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.7500 * inch"}
                            }
                        }
                    }
                },
                "20 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7188 * inch", "holeDiameter" : "0.781 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.7500 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.7031 * inch", "holeDiameter" : "0.781 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.7500 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7188 * inch", "holeDiameter" : "0.906 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.7500 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.7031 * inch", "holeDiameter" : "0.906 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.7500 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7188 * inch", "holeDiameter" : "0.812 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.7500 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.7031 * inch", "holeDiameter" : "0.812 * inch", "cBoreDiameter" : "1 3/16 * inch", "cBoreDepth" : "0.75 * inch", "cSinkDiameter" : "1.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.7500 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7969 * inch", "holeDiameter" : "0.906 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.8750 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.7656 * inch", "holeDiameter" : "0.906 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.8750 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7969 * inch", "holeDiameter" : "1.031 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.8750 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.7656 * inch", "holeDiameter" : "1.031 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.8750 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.7969 * inch", "holeDiameter" : "0.938 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.8750 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.7656 * inch", "holeDiameter" : "0.938 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.8750 * inch"}
                            }
                        }
                    }
                },
                "14 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.8281 * inch", "holeDiameter" : "0.906 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.8750 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.8125 * inch", "holeDiameter" : "0.906 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.8750 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.8281 * inch", "holeDiameter" : "1.031 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.8750 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.8125 * inch", "holeDiameter" : "1.031 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.8750 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.8281 * inch", "holeDiameter" : "0.938 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.8750 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.8125 * inch", "holeDiameter" : "0.938 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.8750 * inch"}
                            }
                        }
                    }
                },
                "20 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.8438 * inch", "holeDiameter" : "0.906 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.8750 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.8281 * inch", "holeDiameter" : "0.906 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.8750 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.8438 * inch", "holeDiameter" : "1.031 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.8750 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.8281 * inch", "holeDiameter" : "1.031 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.8750 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.8438 * inch", "holeDiameter" : "0.938 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.8750 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.8281 * inch", "holeDiameter" : "0.938 * inch", "cBoreDiameter" : "1 3/8 * inch", "cBoreDepth" : "0.875 * inch", "cSinkDiameter" : "1.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "0.8750 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9219 * inch", "holeDiameter" : "1.031 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.0000 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.875 * inch", "holeDiameter" : "1.031 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.0000 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9219 * inch", "holeDiameter" : "1.156 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.0000 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.875 * inch", "holeDiameter" : "1.156 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.0000 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9219 * inch", "holeDiameter" : "1.094 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.0000 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.875 * inch", "holeDiameter" : "1.094 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.0000 * inch"}
                            }
                        }
                    }
                },
                "12 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9531 * inch", "holeDiameter" : "1.031 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.0000 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.9219 * inch", "holeDiameter" : "1.031 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.0000 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9531 * inch", "holeDiameter" : "1.156 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.0000 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.9219 * inch", "holeDiameter" : "1.156 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.0000 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9531 * inch", "holeDiameter" : "1.094 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.0000 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.9219 * inch", "holeDiameter" : "1.094 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.0000 * inch"}
                            }
                        }
                    }
                },
                "20 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9688 * inch", "holeDiameter" : "1.031 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.0000 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.9531 * inch", "holeDiameter" : "1.031 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.0000 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9688 * inch", "holeDiameter" : "1.156 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.0000 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.9531 * inch", "holeDiameter" : "1.156 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.0000 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9688 * inch", "holeDiameter" : "1.094 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.0000 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.9531 * inch", "holeDiameter" : "1.094 * inch", "cBoreDiameter" : "1 5/8 * inch", "cBoreDepth" : "1 * inch", "cSinkDiameter" : "1.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.0000 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.0313 * inch", "holeDiameter" : "1.1406 * inch", "cBoreDiameter" : "1.8125 * inch", "cBoreDepth" : "1.125 * inch", "cSinkDiameter" : "2.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.125 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.9844 * inch", "holeDiameter" : "1.1406 * inch", "cBoreDiameter" : "1.8125 * inch", "cBoreDepth" : "1.125 * inch", "cSinkDiameter" : "2.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.125 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.0313 * inch", "holeDiameter" : "1.1719 * inch", "cBoreDiameter" : "1.8125 * inch", "cBoreDepth" : "1.125 * inch", "cSinkDiameter" : "2.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.125 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.9844 * inch", "holeDiameter" : "1.1719 * inch", "cBoreDiameter" : "1.8125 * inch", "cBoreDepth" : "1.125 * inch", "cSinkDiameter" : "2.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.125 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.0313 * inch", "holeDiameter" : "1.1562 * inch", "cBoreDiameter" : "1.8125 * inch", "cBoreDepth" : "1.125 * inch", "cSinkDiameter" : "2.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.125 * inch"},
                                "75%" : {"tapDrillDiameter" : "0.9844 * inch", "holeDiameter" : "1.1562 * inch", "cBoreDiameter" : "1.8125 * inch", "cBoreDepth" : "1.125 * inch", "cSinkDiameter" : "2.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.125 * inch"}
                            }
                        }
                    }
                },
                "12 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.0781 * inch", "holeDiameter" : "1.1406 * inch", "cBoreDiameter" : "1.8125 * inch", "cBoreDepth" : "1.125 * inch", "cSinkDiameter" : "2.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.125 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.0469 * inch", "holeDiameter" : "1.1406 * inch", "cBoreDiameter" : "1.8125 * inch", "cBoreDepth" : "1.125 * inch", "cSinkDiameter" : "2.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.125 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.0781 * inch", "holeDiameter" : "1.1719 * inch", "cBoreDiameter" : "1.8125 * inch", "cBoreDepth" : "1.125 * inch", "cSinkDiameter" : "2.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.125 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.0469 * inch", "holeDiameter" : "1.1719 * inch", "cBoreDiameter" : "1.8125 * inch", "cBoreDepth" : "1.125 * inch", "cSinkDiameter" : "2.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.125 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.0781 * inch", "holeDiameter" : "1.1562 * inch", "cBoreDiameter" : "1.8125 * inch", "cBoreDepth" : "1.125 * inch", "cSinkDiameter" : "2.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.125 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.0469 * inch", "holeDiameter" : "1.1562 * inch", "cBoreDiameter" : "1.8125 * inch", "cBoreDepth" : "1.125 * inch", "cSinkDiameter" : "2.188 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.125 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.1562 * inch", "holeDiameter" : "1.281 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.2500 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.1094 * inch", "holeDiameter" : "1.281 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.2500 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.1562 * inch", "holeDiameter" : "1.438 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.2500 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.1094 * inch", "holeDiameter" : "1.438 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.2500 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.1562 * inch", "holeDiameter" : "1.344 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.2500 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.1094 * inch", "holeDiameter" : "1.344 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.2500 * inch"}
                            }
                        }
                    }
                },
                "12 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2031 * inch", "holeDiameter" : "1.281 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.2500 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.1719 * inch", "holeDiameter" : "1.281 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.2500 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2031 * inch", "holeDiameter" : "1.438 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.2500 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.1719 * inch", "holeDiameter" : "1.438 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.2500 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2031 * inch", "holeDiameter" : "1.344 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.2500 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.1719 * inch", "holeDiameter" : "1.344 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.2500 * inch"}
                            }
                        }
                    }
                },
                "18 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2187 * inch", "holeDiameter" : "1.281 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.2500 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.1875 * inch", "holeDiameter" : "1.281 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.2500 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2187 * inch", "holeDiameter" : "1.438 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.2500 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.1875 * inch", "holeDiameter" : "1.438 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.2500 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2187 * inch", "holeDiameter" : "1.344 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.2500 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.1875 * inch", "holeDiameter" : "1.344 * inch", "cBoreDiameter" : "2 * inch", "cBoreDepth" : "1.2500 * inch", "cSinkDiameter" : "2.438 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.2500 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2656 * inch", "holeDiameter" : "1.3906 * inch", "cBoreDiameter" : "2.1875 * inch", "cBoreDepth" : "1.375 * inch", "cSinkDiameter" : "2.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.375 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.2187 * inch", "holeDiameter" : "1.3906 * inch", "cBoreDiameter" : "2.1875 * inch", "cBoreDepth" : "1.375 * inch", "cSinkDiameter" : "2.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.375 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2656 * inch", "holeDiameter" : "1.4219 * inch", "cBoreDiameter" : "2.1875 * inch", "cBoreDepth" : "1.375 * inch", "cSinkDiameter" : "2.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.375 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.2187 * inch", "holeDiameter" : "1.4219 * inch", "cBoreDiameter" : "2.1875 * inch", "cBoreDepth" : "1.375 * inch", "cSinkDiameter" : "2.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.375 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2656 * inch", "holeDiameter" : "1.4062 * inch", "cBoreDiameter" : "2.1875 * inch", "cBoreDepth" : "1.375 * inch", "cSinkDiameter" : "2.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.375 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.2187 * inch", "holeDiameter" : "1.4062 * inch", "cBoreDiameter" : "2.1875 * inch", "cBoreDepth" : "1.375 * inch", "cSinkDiameter" : "2.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.375 * inch"}
                            }
                        }
                    }
                },
                "12 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.3281 * inch", "holeDiameter" : "1.3906 * inch", "cBoreDiameter" : "2.1875 * inch", "cBoreDepth" : "1.375 * inch", "cSinkDiameter" : "2.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.375 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.2969 * inch", "holeDiameter" : "1.3906 * inch", "cBoreDiameter" : "2.1875 * inch", "cBoreDepth" : "1.375 * inch", "cSinkDiameter" : "2.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.375 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.3281 * inch", "holeDiameter" : "1.4219 * inch", "cBoreDiameter" : "2.1875 * inch", "cBoreDepth" : "1.375 * inch", "cSinkDiameter" : "2.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.375 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.2969 * inch", "holeDiameter" : "1.4219 * inch", "cBoreDiameter" : "2.1875 * inch", "cBoreDepth" : "1.375 * inch", "cSinkDiameter" : "2.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.375 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.3281 * inch", "holeDiameter" : "1.4062 * inch", "cBoreDiameter" : "2.1875 * inch", "cBoreDepth" : "1.375 * inch", "cSinkDiameter" : "2.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.375 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.2969 * inch", "holeDiameter" : "1.4062 * inch", "cBoreDiameter" : "2.1875 * inch", "cBoreDepth" : "1.375 * inch", "cSinkDiameter" : "2.688 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.375 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.3906 * inch", "holeDiameter" : "1.562 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.5000 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.3437 * inch", "holeDiameter" : "1.562 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.5000 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.3906 * inch", "holeDiameter" : "1.734 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.5000 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.3437 * inch", "holeDiameter" : "1.734 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.5000 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.3906 * inch", "holeDiameter" : "1.625 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.5000 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.3437 * inch", "holeDiameter" : "1.625 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.5000 * inch"}
                            }
                        }
                    }
                },
                "12 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.4375 * inch", "holeDiameter" : "1.562 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.5000 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.4219 * inch", "holeDiameter" : "1.562 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.5000 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.4375 * inch", "holeDiameter" : "1.734 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.5000 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.4219 * inch", "holeDiameter" : "1.734 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.5000 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.4375 * inch", "holeDiameter" : "1.625 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.5000 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.4219 * inch", "holeDiameter" : "1.625 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.5000 * inch"}
                            }
                        }
                    }
                },
                "18 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.4687 * inch", "holeDiameter" : "1.562 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.5000 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.4375 * inch", "holeDiameter" : "1.562 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.5000 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.4687 * inch", "holeDiameter" : "1.734 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.5000 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.4375 * inch", "holeDiameter" : "1.734 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.5000 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.4687 * inch", "holeDiameter" : "1.625 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.5000 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.4375 * inch", "holeDiameter" : "1.625 * inch", "cBoreDiameter" : "2 3/8 * inch", "cBoreDepth" : "1.5000 * inch", "cSinkDiameter" : "2.938 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.5000 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.6417 * inch", "holeDiameter" : "1.7656 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.7500 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.5625 * inch", "holeDiameter" : "1.7656 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.7500 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.6417 * inch", "holeDiameter" : "1.7969 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.7500 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.5625 * inch", "holeDiameter" : "1.7969 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.7500 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.6417 * inch", "holeDiameter" : "1.7812 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.7500 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.5625 * inch", "holeDiameter" : "1.7812 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.7500 * inch"}
                            }
                        }
                    }
                },
                "12 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.6959 * inch", "holeDiameter" : "1.7656 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.7500 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.6735 * inch", "holeDiameter" : "1.7656 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.7500 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.6959 * inch", "holeDiameter" : "1.7969 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.7500 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.6735 * inch", "holeDiameter" : "1.7969 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.7500 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.6959 * inch", "holeDiameter" : "1.7812 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.7500 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.6735 * inch", "holeDiameter" : "1.7812 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.7500 * inch"}
                            }
                        }
                    }
                },
                "16 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.7094 * inch", "holeDiameter" : "1.7656 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.7500 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.6925 * inch", "holeDiameter" : "1.7656 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.7500 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.7094 * inch", "holeDiameter" : "1.7969 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.7500 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.6925 * inch", "holeDiameter" : "1.7969 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.7500 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.7094 * inch", "holeDiameter" : "1.7812 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.7500 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.6925 * inch", "holeDiameter" : "1.7812 * inch", "cBoreDiameter" : "2 3/4 * inch", "cBoreDepth" : "1.7500 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "1.7500 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.8594 * inch", "holeDiameter" : "2.0156 * inch", "cBoreDiameter" : "3.125 * inch", "cBoreDepth" : "2.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.0 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.7812 * inch", "holeDiameter" : "2.0156 * inch", "cBoreDiameter" : "3.125 * inch", "cBoreDepth" : "2.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.0 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.8594 * inch", "holeDiameter" : "2.0469 * inch", "cBoreDiameter" : "3.125 * inch", "cBoreDepth" : "2.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.0 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.7812 * inch", "holeDiameter" : "2.0469 * inch", "cBoreDiameter" : "3.125 * inch", "cBoreDepth" : "2.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.0 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.8594 * inch", "holeDiameter" : "2.0312 * inch", "cBoreDiameter" : "3.125 * inch", "cBoreDepth" : "2.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.0 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.7812 * inch", "holeDiameter" : "2.0312 * inch", "cBoreDiameter" : "3.125 * inch", "cBoreDepth" : "2.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.0 * inch"}
                            }
                        }
                    }
                },
                "12 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.9531 * inch", "holeDiameter" : "2.0156 * inch", "cBoreDiameter" : "3.125 * inch", "cBoreDepth" : "2.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.0 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.9219 * inch", "holeDiameter" : "2.0156 * inch", "cBoreDiameter" : "3.125 * inch", "cBoreDepth" : "2.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.0 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.9531 * inch", "holeDiameter" : "2.0469 * inch", "cBoreDiameter" : "3.125 * inch", "cBoreDepth" : "2.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.0 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.9219 * inch", "holeDiameter" : "2.0469 * inch", "cBoreDiameter" : "3.125 * inch", "cBoreDepth" : "2.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.0 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.9531 * inch", "holeDiameter" : "2.0312 * inch", "cBoreDiameter" : "3.125 * inch", "cBoreDepth" : "2.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.0 * inch"},
                                "75%" : {"tapDrillDiameter" : "1.9219 * inch", "holeDiameter" : "2.0312 * inch", "cBoreDiameter" : "3.125 * inch", "cBoreDepth" : "2.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.0 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.1057 * inch", "holeDiameter" : "2.2656 * inch", "cBoreDiameter" : "3.5 * inch", "cBoreDepth" : "2.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.25 * inch"},
                                "75%" : {"tapDrillDiameter" : "2.036 * inch", "holeDiameter" : "2.2656 * inch", "cBoreDiameter" : "3.5 * inch", "cBoreDepth" : "2.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.25 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.1057 * inch", "holeDiameter" : "2.2969 * inch", "cBoreDiameter" : "3.5 * inch", "cBoreDepth" : "2.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.25 * inch"},
                                "75%" : {"tapDrillDiameter" : "2.036 * inch", "holeDiameter" : "2.2969 * inch", "cBoreDiameter" : "3.5 * inch", "cBoreDepth" : "2.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.25 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.1057 * inch", "holeDiameter" : "2.2812 * inch", "cBoreDiameter" : "3.5 * inch", "cBoreDepth" : "2.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.25 * inch"},
                                "75%" : {"tapDrillDiameter" : "2.036 * inch", "holeDiameter" : "2.2812 * inch", "cBoreDiameter" : "3.5 * inch", "cBoreDepth" : "2.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.25 * inch"}
                            }
                        }
                    }
                },
                "12 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.1959 * inch", "holeDiameter" : "2.2656 * inch", "cBoreDiameter" : "3.5 * inch", "cBoreDepth" : "2.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.25 * inch"},
                                "75%" : {"tapDrillDiameter" : "2.1735 * inch", "holeDiameter" : "2.2656 * inch", "cBoreDiameter" : "3.5 * inch", "cBoreDepth" : "2.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.25 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.1959 * inch", "holeDiameter" : "2.2969 * inch", "cBoreDiameter" : "3.5 * inch", "cBoreDepth" : "2.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.25 * inch"},
                                "75%" : {"tapDrillDiameter" : "2.1735 * inch", "holeDiameter" : "2.2969 * inch", "cBoreDiameter" : "3.5 * inch", "cBoreDepth" : "2.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.25 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.1959 * inch", "holeDiameter" : "2.2812 * inch", "cBoreDiameter" : "3.5 * inch", "cBoreDepth" : "2.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.25 * inch"},
                                "75%" : {"tapDrillDiameter" : "2.1735 * inch", "holeDiameter" : "2.2812 * inch", "cBoreDiameter" : "3.5 * inch", "cBoreDepth" : "2.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.25 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.3376 * inch", "holeDiameter" : "2.5156 * inch", "cBoreDiameter" : "3.875 * inch", "cBoreDepth" : "2.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.5 * inch"},
                                "75%" : {"tapDrillDiameter" : "2.2575 * inch", "holeDiameter" : "2.5156 * inch", "cBoreDiameter" : "3.875 * inch", "cBoreDepth" : "2.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.5 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.3376 * inch", "holeDiameter" : "2.5469 * inch", "cBoreDiameter" : "3.875 * inch", "cBoreDepth" : "2.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.5 * inch"},
                                "75%" : {"tapDrillDiameter" : "2.2575 * inch", "holeDiameter" : "2.5469 * inch", "cBoreDiameter" : "3.875 * inch", "cBoreDepth" : "2.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.5 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.3376 * inch", "holeDiameter" : "2.5312 * inch", "cBoreDiameter" : "3.875 * inch", "cBoreDepth" : "2.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.5 * inch"},
                                "75%" : {"tapDrillDiameter" : "2.2575 * inch", "holeDiameter" : "2.5312 * inch", "cBoreDiameter" : "3.875 * inch", "cBoreDepth" : "2.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.5 * inch"}
                            }
                        }
                    }
                },
                "12 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.4459 * inch", "holeDiameter" : "2.5156 * inch", "cBoreDiameter" : "3.875 * inch", "cBoreDepth" : "2.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.5 * inch"},
                                "75%" : {"tapDrillDiameter" : "2.435 * inch", "holeDiameter" : "2.5156 * inch", "cBoreDiameter" : "3.875 * inch", "cBoreDepth" : "2.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.5 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.4459 * inch", "holeDiameter" : "2.5469 * inch", "cBoreDiameter" : "3.875 * inch", "cBoreDepth" : "2.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.5 * inch"},
                                "75%" : {"tapDrillDiameter" : "2.435 * inch", "holeDiameter" : "2.5469 * inch", "cBoreDiameter" : "3.875 * inch", "cBoreDepth" : "2.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.5 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.4459 * inch", "holeDiameter" : "2.5312 * inch", "cBoreDiameter" : "3.875 * inch", "cBoreDepth" : "2.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.5 * inch"},
                                "75%" : {"tapDrillDiameter" : "2.435 * inch", "holeDiameter" : "2.5312 * inch", "cBoreDiameter" : "3.875 * inch", "cBoreDepth" : "2.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.5 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.5876 * inch", "holeDiameter" : "2.7656 * inch", "cBoreDiameter" : "4.25 * inch", "cBoreDepth" : "2.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.75 * inch"},
                                "75%" : {"tapDrillDiameter" : "2.5075 * inch", "holeDiameter" : "2.7656 * inch", "cBoreDiameter" : "4.25 * inch", "cBoreDepth" : "2.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.75 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.5876 * inch", "holeDiameter" : "2.7969 * inch", "cBoreDiameter" : "4.25 * inch", "cBoreDepth" : "2.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.75 * inch"},
                                "75%" : {"tapDrillDiameter" : "2.5075 * inch", "holeDiameter" : "2.7969 * inch", "cBoreDiameter" : "4.25 * inch", "cBoreDepth" : "2.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.75 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.5876 * inch", "holeDiameter" : "2.7812 * inch", "cBoreDiameter" : "4.25 * inch", "cBoreDepth" : "2.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.75 * inch"},
                                "75%" : {"tapDrillDiameter" : "2.5075 * inch", "holeDiameter" : "2.7812 * inch", "cBoreDiameter" : "4.25 * inch", "cBoreDepth" : "2.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.75 * inch"}
                            }
                        }
                    }
                },
                "12 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.6959 * inch", "holeDiameter" : "2.7656 * inch", "cBoreDiameter" : "4.25 * inch", "cBoreDepth" : "2.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.75 * inch"},
                                "75%" : {"tapDrillDiameter" : "2.6735 * inch", "holeDiameter" : "2.7656 * inch", "cBoreDiameter" : "4.25 * inch", "cBoreDepth" : "2.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.75 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.6959 * inch", "holeDiameter" : "2.7969 * inch", "cBoreDiameter" : "4.25 * inch", "cBoreDepth" : "2.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.75 * inch"},
                                "75%" : {"tapDrillDiameter" : "2.6735 * inch", "holeDiameter" : "2.7969 * inch", "cBoreDiameter" : "4.25 * inch", "cBoreDepth" : "2.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.75 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.6959 * inch", "holeDiameter" : "2.7812 * inch", "cBoreDiameter" : "4.25 * inch", "cBoreDepth" : "2.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.75 * inch"},
                                "75%" : {"tapDrillDiameter" : "2.6735 * inch", "holeDiameter" : "2.7812 * inch", "cBoreDiameter" : "4.25 * inch", "cBoreDepth" : "2.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "2.75 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.8376 * inch", "holeDiameter" : "3.0156 * inch", "cBoreDiameter" : "4.625 * inch", "cBoreDepth" : "3.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.0 * inch"},
                                "75%" : {"tapDrillDiameter" : "2.7575 * inch", "holeDiameter" : "3.0156 * inch", "cBoreDiameter" : "4.625 * inch", "cBoreDepth" : "3.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.0 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.8376 * inch", "holeDiameter" : "3.0469 * inch", "cBoreDiameter" : "4.625 * inch", "cBoreDepth" : "3.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.0 * inch"},
                                "75%" : {"tapDrillDiameter" : "2.7575 * inch", "holeDiameter" : "3.0469 * inch", "cBoreDiameter" : "4.625 * inch", "cBoreDepth" : "3.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.0 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.8376 * inch", "holeDiameter" : "3.0312 * inch", "cBoreDiameter" : "4.625 * inch", "cBoreDepth" : "3.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.0 * inch"},
                                "75%" : {"tapDrillDiameter" : "2.7575 * inch", "holeDiameter" : "3.0312 * inch", "cBoreDiameter" : "4.625 * inch", "cBoreDepth" : "3.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.0 * inch"}
                            }
                        }
                    }
                },
                "12 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.9459 * inch", "holeDiameter" : "3.0156 * inch", "cBoreDiameter" : "4.625 * inch", "cBoreDepth" : "3.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.0 * inch"},
                                "75%" : {"tapDrillDiameter" : "2.9235 * inch", "holeDiameter" : "3.0156 * inch", "cBoreDiameter" : "4.625 * inch", "cBoreDepth" : "3.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.0 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.9459 * inch", "holeDiameter" : "3.0469 * inch", "cBoreDiameter" : "4.625 * inch", "cBoreDepth" : "3.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.0 * inch"},
                                "75%" : {"tapDrillDiameter" : "2.9235 * inch", "holeDiameter" : "3.0469 * inch", "cBoreDiameter" : "4.625 * inch", "cBoreDepth" : "3.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.0 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.9459 * inch", "holeDiameter" : "3.0312 * inch", "cBoreDiameter" : "4.625 * inch", "cBoreDepth" : "3.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.0 * inch"},
                                "75%" : {"tapDrillDiameter" : "2.9235 * inch", "holeDiameter" : "3.0312 * inch", "cBoreDiameter" : "4.625 * inch", "cBoreDepth" : "3.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.0 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.0876 * inch", "holeDiameter" : "3.2656 * inch", "cBoreDiameter" : "5.0 * inch", "cBoreDepth" : "3.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.25 * inch"},
                                "75%" : {"tapDrillDiameter" : "3.0075 * inch", "holeDiameter" : "3.2656 * inch", "cBoreDiameter" : "5.0 * inch", "cBoreDepth" : "3.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.25 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.0876 * inch", "holeDiameter" : "3.2969 * inch", "cBoreDiameter" : "5.0 * inch", "cBoreDepth" : "3.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.25 * inch"},
                                "75%" : {"tapDrillDiameter" : "3.0075 * inch", "holeDiameter" : "3.2969 * inch", "cBoreDiameter" : "5.0 * inch", "cBoreDepth" : "3.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.25 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.0876 * inch", "holeDiameter" : "3.2812 * inch", "cBoreDiameter" : "5.0 * inch", "cBoreDepth" : "3.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.25 * inch"},
                                "75%" : {"tapDrillDiameter" : "3.0075 * inch", "holeDiameter" : "3.2812 * inch", "cBoreDiameter" : "5.0 * inch", "cBoreDepth" : "3.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.25 * inch"}
                            }
                        }
                    }
                },
                "12 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.1959 * inch", "holeDiameter" : "3.2656 * inch", "cBoreDiameter" : "5.0 * inch", "cBoreDepth" : "3.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.25 * inch"},
                                "75%" : {"tapDrillDiameter" : "3.1735 * inch", "holeDiameter" : "3.2656 * inch", "cBoreDiameter" : "5.0 * inch", "cBoreDepth" : "3.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.25 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.1959 * inch", "holeDiameter" : "3.2969 * inch", "cBoreDiameter" : "5.0 * inch", "cBoreDepth" : "3.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.25 * inch"},
                                "75%" : {"tapDrillDiameter" : "3.1735 * inch", "holeDiameter" : "3.2969 * inch", "cBoreDiameter" : "5.0 * inch", "cBoreDepth" : "3.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.25 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.1959 * inch", "holeDiameter" : "3.2812 * inch", "cBoreDiameter" : "5.0 * inch", "cBoreDepth" : "3.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.25 * inch"},
                                "75%" : {"tapDrillDiameter" : "3.1735 * inch", "holeDiameter" : "3.2812 * inch", "cBoreDiameter" : "5.0 * inch", "cBoreDepth" : "3.25 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.25 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.3376 * inch", "holeDiameter" : "3.5156 * inch", "cBoreDiameter" : "5.375 * inch", "cBoreDepth" : "3.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.5 * inch"},
                                "75%" : {"tapDrillDiameter" : "3.2575 * inch", "holeDiameter" : "3.5156 * inch", "cBoreDiameter" : "5.375 * inch", "cBoreDepth" : "3.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.5 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.3376 * inch", "holeDiameter" : "3.5469 * inch", "cBoreDiameter" : "5.375 * inch", "cBoreDepth" : "3.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.5 * inch"},
                                "75%" : {"tapDrillDiameter" : "3.2575 * inch", "holeDiameter" : "3.5469 * inch", "cBoreDiameter" : "5.375 * inch", "cBoreDepth" : "3.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.5 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.3376 * inch", "holeDiameter" : "3.5312 * inch", "cBoreDiameter" : "5.375 * inch", "cBoreDepth" : "3.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.5 * inch"},
                                "75%" : {"tapDrillDiameter" : "3.2575 * inch", "holeDiameter" : "3.5312 * inch", "cBoreDiameter" : "5.375 * inch", "cBoreDepth" : "3.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.5 * inch"}
                            }
                        }
                    }
                },
                "12 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.4459 * inch", "holeDiameter" : "3.5156 * inch", "cBoreDiameter" : "5.375 * inch", "cBoreDepth" : "3.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.5 * inch"},
                                "75%" : {"tapDrillDiameter" : "3.4235 * inch", "holeDiameter" : "3.5156 * inch", "cBoreDiameter" : "5.375 * inch", "cBoreDepth" : "3.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.5 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.4459 * inch", "holeDiameter" : "3.5469 * inch", "cBoreDiameter" : "5.375 * inch", "cBoreDepth" : "3.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.5 * inch"},
                                "75%" : {"tapDrillDiameter" : "3.4235 * inch", "holeDiameter" : "3.5469 * inch", "cBoreDiameter" : "5.375 * inch", "cBoreDepth" : "3.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.5 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.4459 * inch", "holeDiameter" : "3.5312 * inch", "cBoreDiameter" : "5.375 * inch", "cBoreDepth" : "3.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.5 * inch"},
                                "75%" : {"tapDrillDiameter" : "3.4235 * inch", "holeDiameter" : "3.5312 * inch", "cBoreDiameter" : "5.375 * inch", "cBoreDepth" : "3.5 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.5 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.5876 * inch", "holeDiameter" : "3.7656 * inch", "cBoreDiameter" : "5.75 * inch", "cBoreDepth" : "3.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.75 * inch"},
                                "75%" : {"tapDrillDiameter" : "3.5075 * inch", "holeDiameter" : "3.7656 * inch", "cBoreDiameter" : "5.75 * inch", "cBoreDepth" : "3.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.75 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.5876 * inch", "holeDiameter" : "3.7969 * inch", "cBoreDiameter" : "5.75 * inch", "cBoreDepth" : "3.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.75 * inch"},
                                "75%" : {"tapDrillDiameter" : "3.5075 * inch", "holeDiameter" : "3.7969 * inch", "cBoreDiameter" : "5.75 * inch", "cBoreDepth" : "3.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.75 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.5876 * inch", "holeDiameter" : "3.7812 * inch", "cBoreDiameter" : "5.75 * inch", "cBoreDepth" : "3.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.75 * inch"},
                                "75%" : {"tapDrillDiameter" : "3.5075 * inch", "holeDiameter" : "3.7812 * inch", "cBoreDiameter" : "5.75 * inch", "cBoreDepth" : "3.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.75 * inch"}
                            }
                        }
                    }
                },
                "12 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.6959 * inch", "holeDiameter" : "3.7656 * inch", "cBoreDiameter" : "5.75 * inch", "cBoreDepth" : "3.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.75 * inch"},
                                "75%" : {"tapDrillDiameter" : "3.6735 * inch", "holeDiameter" : "3.7656 * inch", "cBoreDiameter" : "5.75 * inch", "cBoreDepth" : "3.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.75 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.6959 * inch", "holeDiameter" : "3.7969 * inch", "cBoreDiameter" : "5.75 * inch", "cBoreDepth" : "3.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.75 * inch"},
                                "75%" : {"tapDrillDiameter" : "3.6735 * inch", "holeDiameter" : "3.7969 * inch", "cBoreDiameter" : "5.75 * inch", "cBoreDepth" : "3.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.75 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.6959 * inch", "holeDiameter" : "3.7812 * inch", "cBoreDiameter" : "5.75 * inch", "cBoreDepth" : "3.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.75 * inch"},
                                "75%" : {"tapDrillDiameter" : "3.6735 * inch", "holeDiameter" : "3.7812 * inch", "cBoreDiameter" : "5.75 * inch", "cBoreDepth" : "3.75 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "3.75 * inch"}
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
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.8376 * inch", "holeDiameter" : "4.0156 * inch", "cBoreDiameter" : "6.125 * inch", "cBoreDepth" : "4.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "4.0 * inch"},
                                "75%" : {"tapDrillDiameter" : "3.7575 * inch", "holeDiameter" : "4.0156 * inch", "cBoreDiameter" : "6.125 * inch", "cBoreDepth" : "4.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "4.0 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.8376 * inch", "holeDiameter" : "4.0469 * inch", "cBoreDiameter" : "6.125 * inch", "cBoreDepth" : "4.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "4.0 * inch"},
                                "75%" : {"tapDrillDiameter" : "3.7575 * inch", "holeDiameter" : "4.0469 * inch", "cBoreDiameter" : "6.125 * inch", "cBoreDepth" : "4.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "4.0 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.8376 * inch", "holeDiameter" : "4.0312 * inch", "cBoreDiameter" : "6.125 * inch", "cBoreDepth" : "4.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "4.0 * inch"},
                                "75%" : {"tapDrillDiameter" : "3.7575 * inch", "holeDiameter" : "4.0312 * inch", "cBoreDiameter" : "6.125 * inch", "cBoreDepth" : "4.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "4.0 * inch"}
                            }
                        }
                    }
                },
                "12 tpi" : {
                    "name" : "fit",
                    "displayName" : "Fit",
                    "default" : "Normal",
                    "entries" : {
                        "Close" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.9459 * inch", "holeDiameter" : "4.0156 * inch", "cBoreDiameter" : "6.125 * inch", "cBoreDepth" : "4.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "4.0 * inch"},
                                "75%" : {"tapDrillDiameter" : "3.9235 * inch", "holeDiameter" : "4.0156 * inch", "cBoreDiameter" : "6.125 * inch", "cBoreDepth" : "4.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "4.0 * inch"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.9459 * inch", "holeDiameter" : "4.0469 * inch", "cBoreDiameter" : "6.125 * inch", "cBoreDepth" : "4.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "4.0 * inch"},
                                "75%" : {"tapDrillDiameter" : "3.9235 * inch", "holeDiameter" : "4.0469 * inch", "cBoreDiameter" : "6.125 * inch", "cBoreDepth" : "4.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "4.0 * inch"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.9459 * inch", "holeDiameter" : "4.0312 * inch", "cBoreDiameter" : "6.125 * inch", "cBoreDepth" : "4.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "4.0 * inch"},
                                "75%" : {"tapDrillDiameter" : "3.9235 * inch", "holeDiameter" : "4.0312 * inch", "cBoreDiameter" : "6.125 * inch", "cBoreDepth" : "4.0 * inch", "cSinkDiameter" : "-1 * inch", "cSinkAngle" : "82 * degree", "majorDiameter" : "4.0 * inch"}
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
        "M1" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.1 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.3 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.2 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M1.1" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.2 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.4 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.3 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M1.2" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.3 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.5 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.4 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M1.4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.5 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.8 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.6 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M1.6" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "3.5 * millimeter", "cBoreDepth" : "1.6 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.7 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "3.5 * millimeter", "cBoreDepth" : "1.6 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.0 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "3.5 * millimeter", "cBoreDepth" : "1.6 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.8 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M1.8" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.0 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.2 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.1 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M2" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "4.4 * millimeter", "cBoreDepth" : "2 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.2 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "4.4 * millimeter", "cBoreDepth" : "2 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.6 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "4.4 * millimeter", "cBoreDepth" : "2 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.4 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M2.2" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.4 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.8 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.6 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M2.5" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "5.5 * millimeter", "cBoreDepth" : "2.5 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.7 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "5.5 * millimeter", "cBoreDepth" : "2.5 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "3.1 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "5.5 * millimeter", "cBoreDepth" : "2.5 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.9 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M3" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.2 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.6 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.4 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M4" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.3 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.8 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.5 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M4.5" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "4.8 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "5.3 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "5.0 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M5" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.3 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.8 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.5 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M6" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "6.4 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "7 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "6.6 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M7" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "7.4 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "8 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "7.6 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M8" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "8.4 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "10 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "9 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M10" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "10.5 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "12 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "11 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M12" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "13 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "14.5 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "13.5 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M14" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "15 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "16.5 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "15.5 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M16" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "17 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "18.5 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "17.5 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M18" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "19 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "21 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "20 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M20" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "21 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "24 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "22 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M22" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "23 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "26 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "24 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M24" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "40 * millimeter", "cBoreDepth" : "24.8 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "25 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "40 * millimeter", "cBoreDepth" : "24.8 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "28 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "40 * millimeter", "cBoreDepth" : "24.8 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "26 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M27" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "28 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "32 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "30 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M30" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "50 * millimeter", "cBoreDepth" : "31 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "31 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "50 * millimeter", "cBoreDepth" : "31 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "35 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "50 * millimeter", "cBoreDepth" : "31 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "33 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M33" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "34 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "38 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "36 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M36" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "58 * millimeter", "cBoreDepth" : "37 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "37 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "58 * millimeter", "cBoreDepth" : "37 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "42 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "58 * millimeter", "cBoreDepth" : "37 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "39 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M39" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "40 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "45 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "42 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M42" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "69.3 * millimeter", "cBoreDepth" : "43 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "43 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "69.3 * millimeter", "cBoreDepth" : "43 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "48 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "69.3 * millimeter", "cBoreDepth" : "43 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "45 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M45" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "46 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "52 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "48 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M48" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "79.2 * millimeter", "cBoreDepth" : "49 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "50 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "79.2 * millimeter", "cBoreDepth" : "49 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "56 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "79.2 * millimeter", "cBoreDepth" : "49 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "52 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M52" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "54 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "62 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "56 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M56" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "92.4 * millimeter", "cBoreDepth" : "57 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "58 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "92.4 * millimeter", "cBoreDepth" : "57 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "66 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "92.4 * millimeter", "cBoreDepth" : "57 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "62 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M60" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "62 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "70 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "66 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M64" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "105.6 * millimeter", "cBoreDepth" : "65 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "66 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "105.6 * millimeter", "cBoreDepth" : "65 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "74 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "105.6 * millimeter", "cBoreDepth" : "65 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "70 * millimeter", "cSinkAngle" : "90 * degree"}
            }
        },
        "M68" : {
            "name" : "fit",
            "displayName" : "Fit",
            "default" : "Normal",
            "entries" : {
                "Close" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "70 * millimeter", "cSinkAngle" : "90 * degree"},
                "Loose" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "78 * millimeter", "cSinkAngle" : "90 * degree"},
                "Normal" : {"cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "74 * millimeter", "cSinkAngle" : "90 * degree"}
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
                        "50%" : {"holeDiameter" : "0.9 * millimeter", "tapDrillDiameter" : "0.9 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1 * millimeter"},
                        "75%" : {"holeDiameter" : "0.8 * millimeter", "tapDrillDiameter" : "0.8 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1 * millimeter"}
                    }
                },
                "0.25 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.85 * millimeter", "tapDrillDiameter" : "0.85 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1 * millimeter"},
                        "75%" : {"holeDiameter" : "0.75 * millimeter", "tapDrillDiameter" : "0.75 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1 * millimeter"}
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
                        "50%" : {"holeDiameter" : "1.0 * millimeter", "tapDrillDiameter" : "1.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.1 * millimeter"},
                        "75%" : {"holeDiameter" : "0.9 * millimeter", "tapDrillDiameter" : "0.9 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.1 * millimeter"}
                    }
                },
                "0.25 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "0.95 * millimeter", "tapDrillDiameter" : "0.95 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.1 * millimeter"},
                        "75%" : {"holeDiameter" : "0.85 * millimeter", "tapDrillDiameter" : "0.85 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.1 * millimeter"}
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
                        "50%" : {"holeDiameter" : "1.1 * millimeter", "tapDrillDiameter" : "1.1 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.2 * millimeter"},
                        "75%" : {"holeDiameter" : "1.0 * millimeter", "tapDrillDiameter" : "1.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.2 * millimeter"}
                    }
                },
                "0.25 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.0 * millimeter", "tapDrillDiameter" : "1.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.2 * millimeter"},
                        "75%" : {"holeDiameter" : "0.95 * millimeter", "tapDrillDiameter" : "0.95 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.2 * millimeter"}
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
                        "50%" : {"holeDiameter" : "1.3 * millimeter", "tapDrillDiameter" : "1.3 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.4 * millimeter"},
                        "75%" : {"holeDiameter" : "1.2 * millimeter", "tapDrillDiameter" : "1.2 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.4 * millimeter"}
                    }
                },
                "0.30 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.2 * millimeter", "tapDrillDiameter" : "1.2 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.4 * millimeter"},
                        "75%" : {"holeDiameter" : "1.1 * millimeter", "tapDrillDiameter" : "1.1 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.4 * millimeter"}
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
                        "50%" : {"holeDiameter" : "1.5 * millimeter", "tapDrillDiameter" : "1.5 * millimeter", "cBoreDiameter" : "3.5 * millimeter", "cBoreDepth" : "1.6 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.6 * millimeter"},
                        "75%" : {"holeDiameter" : "1.4 * millimeter", "tapDrillDiameter" : "1.4 * millimeter", "cBoreDiameter" : "3.5 * millimeter", "cBoreDepth" : "1.6 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.6 * millimeter"}
                    }
                },
                "0.35 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.35 * millimeter", "tapDrillDiameter" : "1.35 * millimeter", "cBoreDiameter" : "3.5 * millimeter", "cBoreDepth" : "1.6 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.6 * millimeter"},
                        "75%" : {"holeDiameter" : "1.25 * millimeter", "tapDrillDiameter" : "1.25 * millimeter", "cBoreDiameter" : "3.5 * millimeter", "cBoreDepth" : "1.6 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.6 * millimeter"}
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
                        "50%" : {"holeDiameter" : "1.7 * millimeter", "tapDrillDiameter" : "1.7 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.8 * millimeter"},
                        "75%" : {"holeDiameter" : "1.6 * millimeter", "tapDrillDiameter" : "1.6 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.8 * millimeter"}
                    }
                },
                "0.35 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.55 * millimeter", "tapDrillDiameter" : "1.55 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.8 * millimeter"},
                        "75%" : {"holeDiameter" : "1.45 * millimeter", "tapDrillDiameter" : "1.45 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.8 * millimeter"}
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
                        "50%" : {"holeDiameter" : "1.85 * millimeter", "tapDrillDiameter" : "1.85 * millimeter", "cBoreDiameter" : "4.4 * millimeter", "cBoreDepth" : "2 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2 * millimeter"},
                        "75%" : {"holeDiameter" : "1.75 * millimeter", "tapDrillDiameter" : "1.75 * millimeter", "cBoreDiameter" : "4.4 * millimeter", "cBoreDepth" : "2 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2 * millimeter"}
                    }
                },
                "0.40 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.75 * millimeter", "tapDrillDiameter" : "1.75 * millimeter", "cBoreDiameter" : "4.4 * millimeter", "cBoreDepth" : "2 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2 * millimeter"},
                        "75%" : {"holeDiameter" : "1.6 * millimeter", "tapDrillDiameter" : "1.6 * millimeter", "cBoreDiameter" : "4.4 * millimeter", "cBoreDepth" : "2 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2 * millimeter"}
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
                        "50%" : {"holeDiameter" : "2.0 * millimeter", "tapDrillDiameter" : "2.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.2 * millimeter"},
                        "75%" : {"holeDiameter" : "1.95 * millimeter", "tapDrillDiameter" : "1.95 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.2 * millimeter"}
                    }
                },
                "0.45 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "1.9 * millimeter", "tapDrillDiameter" : "1.9 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.2 * millimeter"},
                        "75%" : {"holeDiameter" : "1.75 * millimeter", "tapDrillDiameter" : "1.75 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.2 * millimeter"}
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
                        "50%" : {"holeDiameter" : "2.3 * millimeter", "tapDrillDiameter" : "2.3 * millimeter", "cBoreDiameter" : "5.5 * millimeter", "cBoreDepth" : "2.5 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.5 * millimeter"},
                        "75%" : {"holeDiameter" : "2.15 * millimeter", "tapDrillDiameter" : "2.15 * millimeter", "cBoreDiameter" : "5.5 * millimeter", "cBoreDepth" : "2.5 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.5 * millimeter"}
                    }
                },
                "0.45 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "2.2 * millimeter", "tapDrillDiameter" : "2.2 * millimeter", "cBoreDiameter" : "5.5 * millimeter", "cBoreDepth" : "2.5 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.5 * millimeter"},
                        "75%" : {"holeDiameter" : "2.05 * millimeter", "tapDrillDiameter" : "2.05 * millimeter", "cBoreDiameter" : "5.5 * millimeter", "cBoreDepth" : "2.5 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.5 * millimeter"}
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
                        "50%" : {"holeDiameter" : "2.8 * millimeter", "tapDrillDiameter" : "2.8 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "3 * millimeter"},
                        "75%" : {"holeDiameter" : "2.65 * millimeter", "tapDrillDiameter" : "2.65 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "3 * millimeter"}
                    }
                },
                "0.50 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "2.7 * millimeter", "tapDrillDiameter" : "2.7 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "3 * millimeter"},
                        "75%" : {"holeDiameter" : "2.5 * millimeter", "tapDrillDiameter" : "2.5 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "3 * millimeter"}
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
                        "50%" : {"holeDiameter" : "3.7 * millimeter", "tapDrillDiameter" : "3.7 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4 * millimeter"},
                        "75%" : {"holeDiameter" : "3.5 * millimeter", "tapDrillDiameter" : "3.5 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4 * millimeter"}
                    }
                },
                "0.70 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "3.5 * millimeter", "tapDrillDiameter" : "3.5 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4 * millimeter"},
                        "75%" : {"holeDiameter" : "3.3 * millimeter", "tapDrillDiameter" : "3.3 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4 * millimeter"}
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
                        "50%" : {"holeDiameter" : "4.2 * millimeter", "tapDrillDiameter" : "4.2 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4.5 * millimeter"},
                        "75%" : {"holeDiameter" : "4.0 * millimeter", "tapDrillDiameter" : "4.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4.5 * millimeter"}
                    }
                },
                "0.75 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "4.0 * millimeter", "tapDrillDiameter" : "4.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4.5 * millimeter"},
                        "75%" : {"holeDiameter" : "3.7 * millimeter", "tapDrillDiameter" : "3.7 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4.5 * millimeter"}
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
                        "50%" : {"holeDiameter" : "4.7 * millimeter", "tapDrillDiameter" : "4.7 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "5 * millimeter"},
                        "75%" : {"holeDiameter" : "4.5 * millimeter", "tapDrillDiameter" : "4.5 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "5 * millimeter"}
                    }
                },
                "0.80 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "4.5 * millimeter", "tapDrillDiameter" : "4.5 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "5 * millimeter"},
                        "75%" : {"holeDiameter" : "4.2 * millimeter", "tapDrillDiameter" : "4.2 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "5 * millimeter"}
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
                        "50%" : {"holeDiameter" : "5.5 * millimeter", "tapDrillDiameter" : "5.5 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "6 * millimeter"},
                        "75%" : {"holeDiameter" : "5.25 * millimeter", "tapDrillDiameter" : "5.25 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "6 * millimeter"}
                    }
                },
                "1.00 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "5.4 * millimeter", "tapDrillDiameter" : "5.4 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "6 * millimeter"},
                        "75%" : {"holeDiameter" : "5 * millimeter", "tapDrillDiameter" : "5 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "6 * millimeter"}
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
                        "50%" : {"holeDiameter" : "6.5 * millimeter", "tapDrillDiameter" : "6.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "7 * millimeter"},
                        "75%" : {"holeDiameter" : "6.2 * millimeter", "tapDrillDiameter" : "6.2 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "7 * millimeter"}
                    }
                },
                "1.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "6.4 * millimeter", "tapDrillDiameter" : "6.4 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "7 * millimeter"},
                        "75%" : {"holeDiameter" : "6.0 * millimeter", "tapDrillDiameter" : "6.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "7 * millimeter"}
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
                        "50%" : {"holeDiameter" : "7.5 * millimeter", "tapDrillDiameter" : "7.5 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "8 * millimeter"},
                        "75%" : {"holeDiameter" : "7.2 * millimeter", "tapDrillDiameter" : "7.2 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "8 * millimeter"}
                    }
                },
                "1.00 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "7.4 * millimeter", "tapDrillDiameter" : "7.4 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "8 * millimeter"},
                        "75%" : {"holeDiameter" : "7 * millimeter", "tapDrillDiameter" : "7 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "8 * millimeter"}
                    }
                },
                "1.25 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "7.2 * millimeter", "tapDrillDiameter" : "7.2 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "8 * millimeter"},
                        "75%" : {"holeDiameter" : "6.8 * millimeter", "tapDrillDiameter" : "6.8 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "8 * millimeter"}
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
                        "50%" : {"holeDiameter" : "9.4 * millimeter", "tapDrillDiameter" : "9.4 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "10 * millimeter"},
                        "75%" : {"holeDiameter" : "9 * millimeter", "tapDrillDiameter" : "9 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "10 * millimeter"}
                    }
                },
                "1.25 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "9.2 * millimeter", "tapDrillDiameter" : "9.2 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "10 * millimeter"},
                        "75%" : {"holeDiameter" : "8.8 * millimeter", "tapDrillDiameter" : "8.8 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "10 * millimeter"}
                    }
                },
                "1.50 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "9 * millimeter", "tapDrillDiameter" : "9 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "10 * millimeter"},
                        "75%" : {"holeDiameter" : "8.5 * millimeter", "tapDrillDiameter" : "8.5 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "10 * millimeter"}
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
                        "50%" : {"holeDiameter" : "11.2 * millimeter", "tapDrillDiameter" : "11.2 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "12 * millimeter"},
                        "75%" : {"holeDiameter" : "10.8 * millimeter", "tapDrillDiameter" : "10.8 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "12 * millimeter"}
                    }
                },
                "1.50 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "11 * millimeter", "tapDrillDiameter" : "11 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "12 * millimeter"},
                        "75%" : {"holeDiameter" : "10.5 * millimeter", "tapDrillDiameter" : "10.5 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "12 * millimeter"}
                    }
                },
                "1.75 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "11.2 * millimeter", "tapDrillDiameter" : "11.2 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "12 * millimeter"},
                        "75%" : {"holeDiameter" : "10.3 * millimeter", "tapDrillDiameter" : "10.3 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "12 * millimeter"}
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
                        "50%" : {"holeDiameter" : "13.2 * millimeter", "tapDrillDiameter" : "13.2 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "14 * millimeter"},
                        "75%" : {"holeDiameter" : "12.8 * millimeter", "tapDrillDiameter" : "12.8 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "14 * millimeter"}
                    }
                },
                "1.50 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "13 * millimeter", "tapDrillDiameter" : "13 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "14 * millimeter"},
                        "75%" : {"holeDiameter" : "12.5 * millimeter", "tapDrillDiameter" : "12.5 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "14 * millimeter"}
                    }
                },
                "2.00 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "12.7 * millimeter", "tapDrillDiameter" : "12.7 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "14 * millimeter"},
                        "75%" : {"holeDiameter" : "12.1 * millimeter", "tapDrillDiameter" : "12.1 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "14 * millimeter"}
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
                        "50%" : {"holeDiameter" : "15 * millimeter", "tapDrillDiameter" : "15 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "16 * millimeter"},
                        "75%" : {"holeDiameter" : "14.5 * millimeter", "tapDrillDiameter" : "14.5 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "16 * millimeter"}
                    }
                },
                "2.00 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "14.75 * millimeter", "tapDrillDiameter" : "14.75 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "16 * millimeter"},
                        "75%" : {"holeDiameter" : "14 * millimeter", "tapDrillDiameter" : "14 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "16 * millimeter"}
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
                        "50%" : {"holeDiameter" : "17 * millimeter", "tapDrillDiameter" : "17 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "18 * millimeter"},
                        "75%" : {"holeDiameter" : "16.5 * millimeter", "tapDrillDiameter" : "16.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "18 * millimeter"}
                    }
                },
                "2.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "16.7 * millimeter", "tapDrillDiameter" : "16.7 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "18 * millimeter"},
                        "75%" : {"holeDiameter" : "16 * millimeter", "tapDrillDiameter" : "16 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "18 * millimeter"}
                    }
                },
                "2.5 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "16.4 * millimeter", "tapDrillDiameter" : "16.4 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "18 * millimeter"},
                        "75%" : {"holeDiameter" : "15.5 * millimeter", "tapDrillDiameter" : "15.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "18 * millimeter"}
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
                        "50%" : {"holeDiameter" : "19 * millimeter", "tapDrillDiameter" : "19 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "20 * millimeter"},
                        "75%" : {"holeDiameter" : "18.5 * millimeter", "tapDrillDiameter" : "18.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "20 * millimeter"}
                    }
                },
                "2.00 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "18.5 * millimeter", "tapDrillDiameter" : "18.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "20 * millimeter"},
                        "75%" : {"holeDiameter" : "18 * millimeter", "tapDrillDiameter" : "18 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "20 * millimeter"}
                    }
                },
                "2.50 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "18.5 * millimeter", "tapDrillDiameter" : "18.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "20 * millimeter"},
                        "75%" : {"holeDiameter" : "17.5 * millimeter", "tapDrillDiameter" : "17.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "20 * millimeter"}
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
                        "50%" : {"holeDiameter" : "21 * millimeter", "tapDrillDiameter" : "21 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "22 * millimeter"},
                        "75%" : {"holeDiameter" : "20.5 * millimeter", "tapDrillDiameter" : "20.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "22 * millimeter"}
                    }
                },
                "2.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "20.5 * millimeter", "tapDrillDiameter" : "20.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "22 * millimeter"},
                        "75%" : {"holeDiameter" : "20 * millimeter", "tapDrillDiameter" : "20 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "22 * millimeter"}
                    }
                },
                "2.5 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "20.5 * millimeter", "tapDrillDiameter" : "20.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "22 * millimeter"},
                        "75%" : {"holeDiameter" : "19.5 * millimeter", "tapDrillDiameter" : "19.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "22 * millimeter"}
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
                        "50%" : {"holeDiameter" : "22.5 * millimeter", "tapDrillDiameter" : "22.5 * millimeter", "cBoreDiameter" : "40 * millimeter", "cBoreDepth" : "24.8 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "24 * millimeter"},
                        "75%" : {"holeDiameter" : "22 * millimeter", "tapDrillDiameter" : "22 * millimeter", "cBoreDiameter" : "40 * millimeter", "cBoreDepth" : "24.8 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "24 * millimeter"}
                    }
                },
                "3.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "22 * millimeter", "tapDrillDiameter" : "22 * millimeter", "cBoreDiameter" : "40 * millimeter", "cBoreDepth" : "24.8 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "24 * millimeter"},
                        "75%" : {"holeDiameter" : "21 * millimeter", "tapDrillDiameter" : "21 * millimeter", "cBoreDiameter" : "40 * millimeter", "cBoreDepth" : "24.8 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "24 * millimeter"}
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
                        "50%" : {"holeDiameter" : "25.5 * millimeter", "tapDrillDiameter" : "25.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "27 * millimeter"},
                        "75%" : {"holeDiameter" : "25 * millimeter", "tapDrillDiameter" : "25 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "27 * millimeter"}
                    }
                },
                "3.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "25 * millimeter", "tapDrillDiameter" : "25 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "27 * millimeter"},
                        "75%" : {"holeDiameter" : "24 * millimeter", "tapDrillDiameter" : "24 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "27 * millimeter"}
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
                        "50%" : {"holeDiameter" : "28.5 * millimeter", "tapDrillDiameter" : "28.5 * millimeter", "cBoreDiameter" : "50 * millimeter", "cBoreDepth" : "31 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "30 * millimeter"},
                        "75%" : {"holeDiameter" : "28 * millimeter", "tapDrillDiameter" : "28 * millimeter", "cBoreDiameter" : "50 * millimeter", "cBoreDepth" : "31 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "30 * millimeter"}
                    }
                },
                "3.5 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "27.5 * millimeter", "tapDrillDiameter" : "27.5 * millimeter", "cBoreDiameter" : "50 * millimeter", "cBoreDepth" : "31 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "30 * millimeter"},
                        "75%" : {"holeDiameter" : "26.5 * millimeter", "tapDrillDiameter" : "26.5 * millimeter", "cBoreDiameter" : "50 * millimeter", "cBoreDepth" : "31 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "30 * millimeter"}
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
                        "50%" : {"holeDiameter" : "31.5 * millimeter", "tapDrillDiameter" : "31.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "33 * millimeter"},
                        "75%" : {"holeDiameter" : "31 * millimeter", "tapDrillDiameter" : "31 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "33 * millimeter"}
                    }
                },
                "3.5 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "30.5 * millimeter", "tapDrillDiameter" : "30.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "33 * millimeter"},
                        "75%" : {"holeDiameter" : "29.5 * millimeter", "tapDrillDiameter" : "29.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "33 * millimeter"}
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
                        "50%" : {"holeDiameter" : "34 * millimeter", "tapDrillDiameter" : "34 * millimeter", "cBoreDiameter" : "58 * millimeter", "cBoreDepth" : "37 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "36 * millimeter"},
                        "75%" : {"holeDiameter" : "33 * millimeter", "tapDrillDiameter" : "33 * millimeter", "cBoreDiameter" : "58 * millimeter", "cBoreDepth" : "37 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "36 * millimeter"}
                    }
                },
                "4.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "33.5 * millimeter", "tapDrillDiameter" : "33.5 * millimeter", "cBoreDiameter" : "58 * millimeter", "cBoreDepth" : "37 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "36 * millimeter"},
                        "75%" : {"holeDiameter" : "32 * millimeter", "tapDrillDiameter" : "32 * millimeter", "cBoreDiameter" : "58 * millimeter", "cBoreDepth" : "37 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "36 * millimeter"}
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
                        "50%" : {"holeDiameter" : "37 * millimeter", "tapDrillDiameter" : "37 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "39 * millimeter"},
                        "75%" : {"holeDiameter" : "36 * millimeter", "tapDrillDiameter" : "36 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "39 * millimeter"}
                    }
                },
                "4.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "36.5 * millimeter", "tapDrillDiameter" : "36.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "39 * millimeter"},
                        "75%" : {"holeDiameter" : "35 * millimeter", "tapDrillDiameter" : "35 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "39 * millimeter"}
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
                        "50%" : {"holeDiameter" : "40 * millimeter", "tapDrillDiameter" : "40 * millimeter", "cBoreDiameter" : "69.3 * millimeter", "cBoreDepth" : "43 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "42 * millimeter"},
                        "75%" : {"holeDiameter" : "39 * millimeter", "tapDrillDiameter" : "39 * millimeter", "cBoreDiameter" : "69.3 * millimeter", "cBoreDepth" : "43 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "42 * millimeter"}
                    }
                },
                "4.5 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "39 * millimeter", "tapDrillDiameter" : "39 * millimeter", "cBoreDiameter" : "69.3 * millimeter", "cBoreDepth" : "43 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "42 * millimeter"},
                        "75%" : {"holeDiameter" : "37.5 * millimeter", "tapDrillDiameter" : "37.5 * millimeter", "cBoreDiameter" : "69.3 * millimeter", "cBoreDepth" : "43 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "42 * millimeter"}
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
                        "50%" : {"holeDiameter" : "43 * millimeter", "tapDrillDiameter" : "43 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "45 * millimeter"},
                        "75%" : {"holeDiameter" : "42 * millimeter", "tapDrillDiameter" : "42 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "45 * millimeter"}
                    }
                },
                "4.5 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "42 * millimeter", "tapDrillDiameter" : "42 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "45 * millimeter"},
                        "75%" : {"holeDiameter" : "40.5 * millimeter", "tapDrillDiameter" : "40.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "45 * millimeter"}
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
                        "50%" : {"holeDiameter" : "46 * millimeter", "tapDrillDiameter" : "46 * millimeter", "cBoreDiameter" : "79.2 * millimeter", "cBoreDepth" : "49 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "48 * millimeter"},
                        "75%" : {"holeDiameter" : "45 * millimeter", "tapDrillDiameter" : "45 * millimeter", "cBoreDiameter" : "79.2 * millimeter", "cBoreDepth" : "49 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "48 * millimeter"}
                    }
                },
                "5.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "45 * millimeter", "tapDrillDiameter" : "45 * millimeter", "cBoreDiameter" : "79.2 * millimeter", "cBoreDepth" : "49 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "48 * millimeter"},
                        "75%" : {"holeDiameter" : "43 * millimeter", "tapDrillDiameter" : "43 * millimeter", "cBoreDiameter" : "79.2 * millimeter", "cBoreDepth" : "49 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "48 * millimeter"}
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
                        "50%" : {"holeDiameter" : "49.5 * millimeter", "tapDrillDiameter" : "49.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "52 * millimeter"},
                        "75%" : {"holeDiameter" : "48 * millimeter", "tapDrillDiameter" : "48 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "52 * millimeter"}
                    }
                },
                "5.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "49 * millimeter", "tapDrillDiameter" : "49 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "52 * millimeter"},
                        "75%" : {"holeDiameter" : "47 * millimeter", "tapDrillDiameter" : "47 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "52 * millimeter"}
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
                        "50%" : {"holeDiameter" : "53.5 * millimeter", "tapDrillDiameter" : "53.5 * millimeter", "cBoreDiameter" : "92.4 * millimeter", "cBoreDepth" : "57 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "56 * millimeter"},
                        "75%" : {"holeDiameter" : "52 * millimeter", "tapDrillDiameter" : "52 * millimeter", "cBoreDiameter" : "92.4 * millimeter", "cBoreDepth" : "57 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "56 * millimeter"}
                    }
                },
                "5.5 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "52.5 * millimeter", "tapDrillDiameter" : "52.5 * millimeter", "cBoreDiameter" : "92.4 * millimeter", "cBoreDepth" : "57 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "56 * millimeter"},
                        "75%" : {"holeDiameter" : "50.5 * millimeter", "tapDrillDiameter" : "50.5 * millimeter", "cBoreDiameter" : "92.4 * millimeter", "cBoreDepth" : "57 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "56 * millimeter"}
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
                        "50%" : {"holeDiameter" : "57.5 * millimeter", "tapDrillDiameter" : "57.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "60 * millimeter"},
                        "75%" : {"holeDiameter" : "56 * millimeter", "tapDrillDiameter" : "56 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "60 * millimeter"}
                    }
                },
                "5.5 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "56.5 * millimeter", "tapDrillDiameter" : "56.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "60 * millimeter"},
                        "75%" : {"holeDiameter" : "54.5 * millimeter", "tapDrillDiameter" : "54.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "60 * millimeter"}
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
                        "50%" : {"holeDiameter" : "61.5 * millimeter", "tapDrillDiameter" : "61.5 * millimeter", "cBoreDiameter" : "105.6 * millimeter", "cBoreDepth" : "65 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "64 * millimeter"},
                        "75%" : {"holeDiameter" : "60 * millimeter", "tapDrillDiameter" : "60 * millimeter", "cBoreDiameter" : "105.6 * millimeter", "cBoreDepth" : "65 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "64 * millimeter"}
                    }
                },
                "6.0 mm" : {
                    "name" : "engagement",
                    "displayName" : "% diametric engagement",
                    "default" : "75%",
                    "entries" : {
                        "50%" : {"holeDiameter" : "60 * millimeter", "tapDrillDiameter" : "60 * millimeter", "cBoreDiameter" : "105.6 * millimeter", "cBoreDepth" : "65 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "64 * millimeter"},
                        "75%" : {"holeDiameter" : "58 * millimeter", "tapDrillDiameter" : "58 * millimeter", "cBoreDiameter" : "105.6 * millimeter", "cBoreDepth" : "65 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "64 * millimeter"}
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
                        "50%" : {"holeDiameter" : "64 * millimeter", "tapDrillDiameter" : "64 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "68 * millimeter"},
                        "75%" : {"holeDiameter" : "62 * millimeter", "tapDrillDiameter" : "62 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "68 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "0.9 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "0.8 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.3 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "0.8 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.3 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.9 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.2 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "0.8 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.2 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "0.85 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "0.75 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.85 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.3 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "0.75 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.3 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.85 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.2 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "0.75 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.2 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "1.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.2 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.1 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "0.9 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.2 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.1 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.1 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "0.9 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.1 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.3 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.1 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "0.9 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.3 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.1 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "0.95 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.2 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.1 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "0.85 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.2 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.1 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.95 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.1 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "0.85 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.1 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "0.95 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.3 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.1 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "0.85 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.3 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.1 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "1.1 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.3 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.2 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.3 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.2 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.1 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.2 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.2 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.1 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.2 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.2 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "1.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.3 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.2 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "0.95 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.3 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.2 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.2 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "0.95 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.2 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.2 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "0.95 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.2 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "1.3 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.4 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.2 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.4 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.3 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.4 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.2 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.4 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.3 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.4 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.2 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.4 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "1.2 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.4 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.1 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.4 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.4 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.1 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.4 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.2 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.4 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.1 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.4 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "1.5 * millimeter", "cBoreDiameter" : "3.5 * millimeter", "cBoreDepth" : "1.6 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.7 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.6 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.4 * millimeter", "cBoreDiameter" : "3.5 * millimeter", "cBoreDepth" : "1.6 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.7 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.6 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.5 * millimeter", "cBoreDiameter" : "3.5 * millimeter", "cBoreDepth" : "1.6 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.0 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.6 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.4 * millimeter", "cBoreDiameter" : "3.5 * millimeter", "cBoreDepth" : "1.6 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.0 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.6 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.5 * millimeter", "cBoreDiameter" : "3.5 * millimeter", "cBoreDepth" : "1.6 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.6 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.4 * millimeter", "cBoreDiameter" : "3.5 * millimeter", "cBoreDepth" : "1.6 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.6 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "1.35 * millimeter", "cBoreDiameter" : "3.5 * millimeter", "cBoreDepth" : "1.6 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.7 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.6 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.25 * millimeter", "cBoreDiameter" : "3.5 * millimeter", "cBoreDepth" : "1.6 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.7 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.6 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.35 * millimeter", "cBoreDiameter" : "3.5 * millimeter", "cBoreDepth" : "1.6 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.0 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.6 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.25 * millimeter", "cBoreDiameter" : "3.5 * millimeter", "cBoreDepth" : "1.6 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.0 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.6 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.35 * millimeter", "cBoreDiameter" : "3.5 * millimeter", "cBoreDepth" : "1.6 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.6 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.25 * millimeter", "cBoreDiameter" : "3.5 * millimeter", "cBoreDepth" : "1.6 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "1.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.6 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "1.7 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.0 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.8 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.6 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.0 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.8 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.7 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.2 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.8 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.6 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.2 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.8 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.7 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.8 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.6 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.8 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "1.55 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.0 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.8 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.45 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.0 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.8 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.55 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.2 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.8 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.45 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.2 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.8 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.55 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.8 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.45 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "1.8 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "1.85 * millimeter", "cBoreDiameter" : "4.4 * millimeter", "cBoreDepth" : "2 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.2 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.75 * millimeter", "cBoreDiameter" : "4.4 * millimeter", "cBoreDepth" : "2 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.2 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.85 * millimeter", "cBoreDiameter" : "4.4 * millimeter", "cBoreDepth" : "2 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.75 * millimeter", "cBoreDiameter" : "4.4 * millimeter", "cBoreDepth" : "2 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.85 * millimeter", "cBoreDiameter" : "4.4 * millimeter", "cBoreDepth" : "2 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.75 * millimeter", "cBoreDiameter" : "4.4 * millimeter", "cBoreDepth" : "2 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "1.75 * millimeter", "cBoreDiameter" : "4.4 * millimeter", "cBoreDepth" : "2 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.2 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.6 * millimeter", "cBoreDiameter" : "4.4 * millimeter", "cBoreDepth" : "2 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.2 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.75 * millimeter", "cBoreDiameter" : "4.4 * millimeter", "cBoreDepth" : "2 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.6 * millimeter", "cBoreDiameter" : "4.4 * millimeter", "cBoreDepth" : "2 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.75 * millimeter", "cBoreDiameter" : "4.4 * millimeter", "cBoreDepth" : "2 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.6 * millimeter", "cBoreDiameter" : "4.4 * millimeter", "cBoreDepth" : "2 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "2.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.2 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.95 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.2 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.2 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.95 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.2 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.2 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.95 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.2 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "1.9 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.2 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.75 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.2 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.9 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.2 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.75 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.2 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "1.9 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.2 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "1.75 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.2 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "2.3 * millimeter", "cBoreDiameter" : "5.5 * millimeter", "cBoreDepth" : "2.5 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.7 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.5 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "2.15 * millimeter", "cBoreDiameter" : "5.5 * millimeter", "cBoreDepth" : "2.5 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.7 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.5 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.3 * millimeter", "cBoreDiameter" : "5.5 * millimeter", "cBoreDepth" : "2.5 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "3.1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.5 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "2.15 * millimeter", "cBoreDiameter" : "5.5 * millimeter", "cBoreDepth" : "2.5 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "3.1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.5 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.3 * millimeter", "cBoreDiameter" : "5.5 * millimeter", "cBoreDepth" : "2.5 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.9 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.5 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "2.15 * millimeter", "cBoreDiameter" : "5.5 * millimeter", "cBoreDepth" : "2.5 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.9 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.5 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "2.2 * millimeter", "cBoreDiameter" : "5.5 * millimeter", "cBoreDepth" : "2.5 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.7 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.5 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "2.05 * millimeter", "cBoreDiameter" : "5.5 * millimeter", "cBoreDepth" : "2.5 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.7 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.5 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.2 * millimeter", "cBoreDiameter" : "5.5 * millimeter", "cBoreDepth" : "2.5 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "3.1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.5 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "2.05 * millimeter", "cBoreDiameter" : "5.5 * millimeter", "cBoreDepth" : "2.5 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "3.1 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.5 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.2 * millimeter", "cBoreDiameter" : "5.5 * millimeter", "cBoreDepth" : "2.5 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.9 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.5 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "2.05 * millimeter", "cBoreDiameter" : "5.5 * millimeter", "cBoreDepth" : "2.5 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "2.9 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "2.5 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "2.8 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.2 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "3 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "2.65 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.2 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "3 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.8 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "3 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "2.65 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "3 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.8 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "3 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "2.65 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "3 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "2.7 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.2 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "3 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "2.5 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.2 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "3 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.7 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "3 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "2.5 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "3 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "2.7 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "3 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "2.5 * millimeter", "cBoreDiameter" : "6.5 * millimeter", "cBoreDepth" : "3 * millimeter", "cSinkDiameter" : "6.72 * millimeter", "holeDiameter" : "3.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "3 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "3.7 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.3 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "3.5 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.3 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.7 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "3.5 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.7 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "3.5 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "3.5 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.3 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "3.3 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.3 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.5 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "3.3 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "3.5 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "3.3 * millimeter", "cBoreDiameter" : "8.25 * millimeter", "cBoreDepth" : "4 * millimeter", "cSinkDiameter" : "8.96 * millimeter", "holeDiameter" : "4.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "4.2 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "4.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4.5 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "4.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "4.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4.5 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "4.2 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "5.3 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4.5 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "4.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "5.3 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4.5 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "4.2 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "5.0 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4.5 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "4.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "5.0 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4.5 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "4.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "4.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4.5 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "3.7 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "4.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4.5 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "4.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "5.3 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4.5 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "3.7 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "5.3 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4.5 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "4.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "5.0 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4.5 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "3.7 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "5.0 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "4.5 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "4.7 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.3 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "5 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "4.5 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.3 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "5 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "4.7 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "5 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "4.5 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "5 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "4.7 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "5 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "4.5 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "5 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "4.5 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.3 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "5 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "4.2 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.3 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "5 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "4.5 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "5 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "4.2 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "5 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "4.5 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "5 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "4.2 * millimeter", "cBoreDiameter" : "9.75 * millimeter", "cBoreDepth" : "5 * millimeter", "cSinkDiameter" : "11.2 * millimeter", "holeDiameter" : "5.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "5 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "5.5 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "6.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "6 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "5.25 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "6.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "6 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "5.5 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "7 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "6 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "5.25 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "7 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "6 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "5.5 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "6.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "6 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "5.25 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "6.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "6 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "5.4 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "6.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "6 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "5 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "6.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "6 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "5.4 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "7 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "6 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "5 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "7 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "6 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "5.4 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "6.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "6 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "5 * millimeter", "cBoreDiameter" : "11.25 * millimeter", "cBoreDepth" : "6 * millimeter", "cSinkDiameter" : "13.44 * millimeter", "holeDiameter" : "6.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "6 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "6.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "7.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "7 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "6.2 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "7.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "7 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "6.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "7 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "6.2 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "7 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "6.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "7.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "7 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "6.2 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "7.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "7 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "6.4 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "7.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "7 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "6.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "7.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "7 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "6.4 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "7 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "6.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "8 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "7 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "6.4 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "7.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "7 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "6.0 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "7.6 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "7 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "7.5 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "8.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "8 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "7.2 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "8.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "8 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "7.5 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "10 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "8 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "7.2 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "10 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "8 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "7.5 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "9 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "8 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "7.2 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "9 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "8 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "7.4 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "8.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "8 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "7 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "8.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "8 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "7.4 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "10 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "8 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "7 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "10 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "8 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "7.4 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "9 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "8 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "7 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "9 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "8 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "7.2 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "8.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "8 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "6.8 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "8.4 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "8 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "7.2 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "10 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "8 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "6.8 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "10 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "8 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "7.2 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "9 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "8 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "6.8 * millimeter", "cBoreDiameter" : "14.25 * millimeter", "cBoreDepth" : "8 * millimeter", "cSinkDiameter" : "17.92 * millimeter", "holeDiameter" : "9 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "8 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "9.4 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "10.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "10 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "9 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "10.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "10 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "9.4 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "12 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "10 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "9 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "12 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "10 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "9.4 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "11 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "10 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "9 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "11 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "10 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "9.2 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "10.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "10 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "8.8 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "10.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "10 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "9.2 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "12 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "10 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "8.8 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "12 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "10 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "9.2 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "11 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "10 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "8.8 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "11 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "10 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "9 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "10.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "10 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "8.5 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "10.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "10 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "9 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "12 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "10 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "8.5 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "12 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "10 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "9 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "11 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "10 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "8.5 * millimeter", "cBoreDiameter" : "17.25 * millimeter", "cBoreDepth" : "10 * millimeter", "cSinkDiameter" : "22.4 * millimeter", "holeDiameter" : "11 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "10 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "11.2 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "13 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "12 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "10.8 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "13 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "12 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "11.2 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "14.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "12 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "10.8 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "14.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "12 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "11.2 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "13.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "12 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "10.8 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "13.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "12 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "11 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "13 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "12 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "10.5 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "13 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "12 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "11 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "14.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "12 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "10.5 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "14.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "12 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "11 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "13.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "12 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "10.5 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "13.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "12 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "11.2 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "13 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "12 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "10.3 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "13 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "12 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "11.2 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "14.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "12 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "10.3 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "14.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "12 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "11.2 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "13.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "12 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "10.3 * millimeter", "cBoreDiameter" : "19.25 * millimeter", "cBoreDepth" : "12 * millimeter", "cSinkDiameter" : "26.88 * millimeter", "holeDiameter" : "13.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "12 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "13.2 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "15 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "14 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "12.8 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "15 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "14 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "13.2 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "16.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "14 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "12.8 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "16.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "14 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "13.2 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "15.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "14 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "12.8 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "15.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "14 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "13 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "15 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "14 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "12.5 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "15 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "14 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "13 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "16.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "14 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "12.5 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "16.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "14 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "13 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "15.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "14 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "12.5 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "15.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "14 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "12.7 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "15 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "14 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "12.1 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "15 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "14 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "12.7 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "16.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "14 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "12.1 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "16.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "14 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "12.7 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "15.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "14 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "12.1 * millimeter", "cBoreDiameter" : "22.25 * millimeter", "cBoreDepth" : "14 * millimeter", "cSinkDiameter" : "30.8 * millimeter", "holeDiameter" : "15.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "14 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "15 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "17 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "16 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "14.5 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "17 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "16 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "15 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "18.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "16 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "14.5 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "18.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "16 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "15 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "17.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "16 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "14.5 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "17.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "16 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "14.75 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "17 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "16 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "14 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "17 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "16 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "14.75 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "18.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "16 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "14 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "18.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "16 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "14.75 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "17.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "16 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "14 * millimeter", "cBoreDiameter" : "25.5 * millimeter", "cBoreDepth" : "16 * millimeter", "cSinkDiameter" : "33.6 * millimeter", "holeDiameter" : "17.5 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "16 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "17 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "19 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "18 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "16.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "19 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "18 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "17 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "21 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "18 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "16.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "21 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "18 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "17 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "20 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "18 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "16.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "20 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "18 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "16.7 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "19 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "18 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "16 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "19 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "18 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "16.7 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "21 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "18 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "16 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "21 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "18 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "16.7 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "20 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "18 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "16 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "20 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "18 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "16.4 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "19 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "18 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "15.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "19 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "18 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "16.4 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "21 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "18 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "15.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "21 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "18 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "16.4 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "20 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "18 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "15.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "20 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "18 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "19 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "21 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "20 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "18.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "21 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "20 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "19 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "24 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "20 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "18.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "24 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "20 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "19 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "22 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "20 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "18.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "22 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "20 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "18.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "21 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "20 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "18 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "21 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "20 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "18.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "24 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "20 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "18 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "24 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "20 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "18.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "22 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "20 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "18 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "22 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "20 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "18.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "21 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "20 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "17.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "21 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "20 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "18.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "24 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "20 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "17.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "24 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "20 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "18.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "22 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "20 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "17.5 * millimeter", "cBoreDiameter" : "31.5 * millimeter", "cBoreDepth" : "20 * millimeter", "cSinkDiameter" : "42 * millimeter", "holeDiameter" : "22 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "20 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "21 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "23 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "22 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "20.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "23 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "22 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "21 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "26 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "22 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "20.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "26 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "22 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "21 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "24 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "22 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "20.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "24 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "22 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "20.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "23 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "22 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "20 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "23 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "22 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "20.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "26 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "22 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "20 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "26 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "22 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "20.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "24 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "22 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "20 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "24 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "22 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "20.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "23 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "22 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "19.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "23 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "22 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "20.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "26 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "22 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "19.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "26 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "22 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "20.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "24 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "22 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "19.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "24 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "22 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "22.5 * millimeter", "cBoreDiameter" : "40 * millimeter", "cBoreDepth" : "24.8 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "25 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "24 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "22 * millimeter", "cBoreDiameter" : "40 * millimeter", "cBoreDepth" : "24.8 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "25 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "24 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "22.5 * millimeter", "cBoreDiameter" : "40 * millimeter", "cBoreDepth" : "24.8 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "28 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "24 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "22 * millimeter", "cBoreDiameter" : "40 * millimeter", "cBoreDepth" : "24.8 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "28 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "24 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "22.5 * millimeter", "cBoreDiameter" : "40 * millimeter", "cBoreDepth" : "24.8 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "26 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "24 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "22 * millimeter", "cBoreDiameter" : "40 * millimeter", "cBoreDepth" : "24.8 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "26 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "24 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "22 * millimeter", "cBoreDiameter" : "40 * millimeter", "cBoreDepth" : "24.8 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "25 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "24 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "21 * millimeter", "cBoreDiameter" : "40 * millimeter", "cBoreDepth" : "24.8 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "25 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "24 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "22 * millimeter", "cBoreDiameter" : "40 * millimeter", "cBoreDepth" : "24.8 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "28 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "24 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "21 * millimeter", "cBoreDiameter" : "40 * millimeter", "cBoreDepth" : "24.8 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "28 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "24 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "22 * millimeter", "cBoreDiameter" : "40 * millimeter", "cBoreDepth" : "24.8 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "26 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "24 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "21 * millimeter", "cBoreDiameter" : "40 * millimeter", "cBoreDepth" : "24.8 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "26 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "24 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "25.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "28 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "27 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "25 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "28 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "27 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "25.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "32 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "27 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "25 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "32 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "27 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "25.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "30 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "27 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "25 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "30 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "27 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "25 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "28 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "27 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "24 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "28 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "27 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "25 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "32 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "27 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "24 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "32 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "27 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "25 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "30 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "27 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "24 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "30 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "27 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "28.5 * millimeter", "cBoreDiameter" : "50 * millimeter", "cBoreDepth" : "31 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "31 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "30 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "28 * millimeter", "cBoreDiameter" : "50 * millimeter", "cBoreDepth" : "31 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "31 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "30 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "28.5 * millimeter", "cBoreDiameter" : "50 * millimeter", "cBoreDepth" : "31 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "35 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "30 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "28 * millimeter", "cBoreDiameter" : "50 * millimeter", "cBoreDepth" : "31 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "35 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "30 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "28.5 * millimeter", "cBoreDiameter" : "50 * millimeter", "cBoreDepth" : "31 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "33 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "30 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "28 * millimeter", "cBoreDiameter" : "50 * millimeter", "cBoreDepth" : "31 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "33 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "30 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "27.5 * millimeter", "cBoreDiameter" : "50 * millimeter", "cBoreDepth" : "31 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "31 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "30 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "26.5 * millimeter", "cBoreDiameter" : "50 * millimeter", "cBoreDepth" : "31 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "31 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "30 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "27.5 * millimeter", "cBoreDiameter" : "50 * millimeter", "cBoreDepth" : "31 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "35 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "30 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "26.5 * millimeter", "cBoreDiameter" : "50 * millimeter", "cBoreDepth" : "31 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "35 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "30 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "27.5 * millimeter", "cBoreDiameter" : "50 * millimeter", "cBoreDepth" : "31 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "33 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "30 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "26.5 * millimeter", "cBoreDiameter" : "50 * millimeter", "cBoreDepth" : "31 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "33 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "30 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "31.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "34 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "33 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "31 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "34 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "33 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "31.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "38 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "33 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "31 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "38 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "33 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "31.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "36 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "33 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "31 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "36 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "33 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "30.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "34 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "33 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "29.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "34 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "33 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "30.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "38 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "33 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "29.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "38 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "33 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "30.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "36 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "33 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "29.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "36 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "33 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "34 * millimeter", "cBoreDiameter" : "58 * millimeter", "cBoreDepth" : "37 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "37 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "36 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "33 * millimeter", "cBoreDiameter" : "58 * millimeter", "cBoreDepth" : "37 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "37 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "36 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "34 * millimeter", "cBoreDiameter" : "58 * millimeter", "cBoreDepth" : "37 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "42 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "36 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "33 * millimeter", "cBoreDiameter" : "58 * millimeter", "cBoreDepth" : "37 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "42 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "36 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "34 * millimeter", "cBoreDiameter" : "58 * millimeter", "cBoreDepth" : "37 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "39 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "36 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "33 * millimeter", "cBoreDiameter" : "58 * millimeter", "cBoreDepth" : "37 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "39 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "36 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "33.5 * millimeter", "cBoreDiameter" : "58 * millimeter", "cBoreDepth" : "37 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "37 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "36 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "32 * millimeter", "cBoreDiameter" : "58 * millimeter", "cBoreDepth" : "37 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "37 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "36 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "33.5 * millimeter", "cBoreDiameter" : "58 * millimeter", "cBoreDepth" : "37 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "42 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "36 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "32 * millimeter", "cBoreDiameter" : "58 * millimeter", "cBoreDepth" : "37 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "42 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "36 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "33.5 * millimeter", "cBoreDiameter" : "58 * millimeter", "cBoreDepth" : "37 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "39 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "36 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "32 * millimeter", "cBoreDiameter" : "58 * millimeter", "cBoreDepth" : "37 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "39 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "36 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "37 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "40 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "39 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "36 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "40 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "39 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "37 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "45 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "39 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "36 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "45 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "39 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "37 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "42 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "39 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "36 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "42 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "39 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "36.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "40 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "39 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "35 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "40 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "39 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "36.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "45 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "39 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "35 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "45 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "39 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "36.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "42 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "39 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "35 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "42 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "39 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "40 * millimeter", "cBoreDiameter" : "69.3 * millimeter", "cBoreDepth" : "43 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "43 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "42 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "39 * millimeter", "cBoreDiameter" : "69.3 * millimeter", "cBoreDepth" : "43 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "43 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "42 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "40 * millimeter", "cBoreDiameter" : "69.3 * millimeter", "cBoreDepth" : "43 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "48 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "42 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "39 * millimeter", "cBoreDiameter" : "69.3 * millimeter", "cBoreDepth" : "43 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "48 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "42 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "40 * millimeter", "cBoreDiameter" : "69.3 * millimeter", "cBoreDepth" : "43 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "45 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "42 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "39 * millimeter", "cBoreDiameter" : "69.3 * millimeter", "cBoreDepth" : "43 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "45 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "42 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "39 * millimeter", "cBoreDiameter" : "69.3 * millimeter", "cBoreDepth" : "43 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "43 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "42 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "37.5 * millimeter", "cBoreDiameter" : "69.3 * millimeter", "cBoreDepth" : "43 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "43 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "42 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "39 * millimeter", "cBoreDiameter" : "69.3 * millimeter", "cBoreDepth" : "43 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "48 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "42 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "37.5 * millimeter", "cBoreDiameter" : "69.3 * millimeter", "cBoreDepth" : "43 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "48 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "42 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "39 * millimeter", "cBoreDiameter" : "69.3 * millimeter", "cBoreDepth" : "43 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "45 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "42 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "37.5 * millimeter", "cBoreDiameter" : "69.3 * millimeter", "cBoreDepth" : "43 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "45 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "42 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "43 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "46 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "45 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "42 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "46 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "45 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "43 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "52 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "45 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "42 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "52 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "45 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "43 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "48 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "45 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "42 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "48 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "45 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "42 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "46 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "45 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "40.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "46 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "45 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "42 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "52 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "45 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "40.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "52 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "45 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "42 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "48 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "45 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "40.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "48 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "45 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "46 * millimeter", "cBoreDiameter" : "79.2 * millimeter", "cBoreDepth" : "49 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "50 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "48 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "45 * millimeter", "cBoreDiameter" : "79.2 * millimeter", "cBoreDepth" : "49 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "50 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "48 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "46 * millimeter", "cBoreDiameter" : "79.2 * millimeter", "cBoreDepth" : "49 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "56 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "48 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "45 * millimeter", "cBoreDiameter" : "79.2 * millimeter", "cBoreDepth" : "49 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "56 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "48 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "46 * millimeter", "cBoreDiameter" : "79.2 * millimeter", "cBoreDepth" : "49 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "52 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "48 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "45 * millimeter", "cBoreDiameter" : "79.2 * millimeter", "cBoreDepth" : "49 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "52 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "48 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "45 * millimeter", "cBoreDiameter" : "79.2 * millimeter", "cBoreDepth" : "49 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "50 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "48 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "43 * millimeter", "cBoreDiameter" : "79.2 * millimeter", "cBoreDepth" : "49 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "50 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "48 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "45 * millimeter", "cBoreDiameter" : "79.2 * millimeter", "cBoreDepth" : "49 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "56 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "48 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "43 * millimeter", "cBoreDiameter" : "79.2 * millimeter", "cBoreDepth" : "49 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "56 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "48 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "45 * millimeter", "cBoreDiameter" : "79.2 * millimeter", "cBoreDepth" : "49 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "52 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "48 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "43 * millimeter", "cBoreDiameter" : "79.2 * millimeter", "cBoreDepth" : "49 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "52 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "48 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "49.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "54 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "52 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "48 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "54 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "52 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "49.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "62 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "52 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "48 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "62 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "52 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "49.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "56 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "52 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "48 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "56 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "52 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "49 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "54 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "52 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "47 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "54 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "52 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "49 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "62 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "52 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "47 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "62 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "52 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "49 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "56 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "52 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "47 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "56 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "52 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "53.5 * millimeter", "cBoreDiameter" : "92.4 * millimeter", "cBoreDepth" : "57 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "58 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "56 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "52 * millimeter", "cBoreDiameter" : "92.4 * millimeter", "cBoreDepth" : "57 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "58 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "56 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "53.5 * millimeter", "cBoreDiameter" : "92.4 * millimeter", "cBoreDepth" : "57 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "66 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "56 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "52 * millimeter", "cBoreDiameter" : "92.4 * millimeter", "cBoreDepth" : "57 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "66 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "56 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "53.5 * millimeter", "cBoreDiameter" : "92.4 * millimeter", "cBoreDepth" : "57 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "62 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "56 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "52 * millimeter", "cBoreDiameter" : "92.4 * millimeter", "cBoreDepth" : "57 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "62 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "56 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "52.5 * millimeter", "cBoreDiameter" : "92.4 * millimeter", "cBoreDepth" : "57 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "58 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "56 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "50.5 * millimeter", "cBoreDiameter" : "92.4 * millimeter", "cBoreDepth" : "57 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "58 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "56 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "52.5 * millimeter", "cBoreDiameter" : "92.4 * millimeter", "cBoreDepth" : "57 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "66 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "56 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "50.5 * millimeter", "cBoreDiameter" : "92.4 * millimeter", "cBoreDepth" : "57 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "66 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "56 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "52.5 * millimeter", "cBoreDiameter" : "92.4 * millimeter", "cBoreDepth" : "57 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "62 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "56 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "50.5 * millimeter", "cBoreDiameter" : "92.4 * millimeter", "cBoreDepth" : "57 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "62 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "56 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "57.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "62 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "60 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "56 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "62 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "60 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "57.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "70 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "60 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "56 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "70 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "60 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "57.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "66 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "60 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "56 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "66 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "60 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "56.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "62 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "60 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "54.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "62 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "60 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "56.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "70 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "60 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "54.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "70 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "60 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "56.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "66 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "60 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "54.5 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "66 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "60 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "61.5 * millimeter", "cBoreDiameter" : "105.6 * millimeter", "cBoreDepth" : "65 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "66 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "64 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "60 * millimeter", "cBoreDiameter" : "105.6 * millimeter", "cBoreDepth" : "65 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "66 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "64 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "61.5 * millimeter", "cBoreDiameter" : "105.6 * millimeter", "cBoreDepth" : "65 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "74 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "64 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "60 * millimeter", "cBoreDiameter" : "105.6 * millimeter", "cBoreDepth" : "65 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "74 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "64 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "61.5 * millimeter", "cBoreDiameter" : "105.6 * millimeter", "cBoreDepth" : "65 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "70 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "64 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "60 * millimeter", "cBoreDiameter" : "105.6 * millimeter", "cBoreDepth" : "65 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "70 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "64 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "60 * millimeter", "cBoreDiameter" : "105.6 * millimeter", "cBoreDepth" : "65 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "66 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "64 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "58 * millimeter", "cBoreDiameter" : "105.6 * millimeter", "cBoreDepth" : "65 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "66 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "64 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "60 * millimeter", "cBoreDiameter" : "105.6 * millimeter", "cBoreDepth" : "65 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "74 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "64 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "58 * millimeter", "cBoreDiameter" : "105.6 * millimeter", "cBoreDepth" : "65 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "74 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "64 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "60 * millimeter", "cBoreDiameter" : "105.6 * millimeter", "cBoreDepth" : "65 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "70 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "64 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "58 * millimeter", "cBoreDiameter" : "105.6 * millimeter", "cBoreDepth" : "65 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "70 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "64 * millimeter"}
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
                                "50%" : {"tapDrillDiameter" : "64 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "70 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "68 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "62 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "70 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "68 * millimeter"}
                            }
                        },
                        "Loose" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "64 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "78 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "68 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "62 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "78 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "68 * millimeter"}
                            }
                        },
                        "Normal" : {
                            "name" : "engagement",
                            "displayName" : "% diametric engagement",
                            "default" : "75%",
                            "entries" : {
                                "50%" : {"tapDrillDiameter" : "64 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "74 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "68 * millimeter"},
                                "75%" : {"tapDrillDiameter" : "62 * millimeter", "cBoreDiameter" : "-1 * millimeter", "cBoreDepth" : "0 * millimeter", "cSinkDiameter" : "-1 * millimeter", "holeDiameter" : "74 * millimeter", "cSinkAngle" : "90 * degree", "majorDiameter" : "68 * millimeter"}
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


