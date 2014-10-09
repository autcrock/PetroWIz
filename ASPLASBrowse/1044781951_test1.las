~Version Information
VERS .            2.00                          : CWLS Log ASCII Standard - Version 2.00
WRAP .            YES                           : Multiple lines per depth step
#
~Well Information
#MNEM.UNIT        DATA                          : DESCRIPTION
#----.----        ----                          : -----------
STRT .F           1649.000                      : Start
STOP .F           5729.976                      : Stop
STEP .F              0.328                      : Step increment
NULL .            -999.250                      : Null value
COMP .            WESTERN OPERATING COMPANY     : Company
WELL .            LMS LAND #1                   : Well
FLD  .            ARROYO                        : Field
CTRY .            United States                 : State / Country
LOC  .            990' FNL & 1980' FWL          : Location
LOC2 .                                          : Location 2
SECT.                       22: SECTION
TOWN.                     29 S: TOWNSHIP
RANG.                     41 W: RANGE
SRVC .            Weatherford                   : Service company
STAT .            U.S.A. / KANSAS               : State
CNTY .            STANTON                       : County
API  .            15-187-21236                  : API Number
LIC  .                                          : Licence Number
DATE .            29-Aug-2013                   : Logging Date
#
~Curve Information
#MNEM.UNIT        API CODES                     : CURVE DESCRIPTION
#----.----        ---------                     : -----------------
DEPT .F           00 001 00 00                  : Logged depth
CGXT .DEGF        30 660 01 00                  : MCG External Temperature
SMTU .LB          30 635 00 00                  : DST Uphole Tension
GRGC .GAPI        30 310 01 00                  : Gamma Ray
NPRL .PERC        42 890 01 00                  : Limestone Neutron Por.
CLDC .IN          45 280 01 00                  : Density Caliper
PDPE .B/E         45 358 01 00                  : PE
DPRL .PERC        45 890 10 00                  : Limestone Density Por.
MNRL .OHMM        15 252 00 00                  : Micro-normal
MINV .OHMM        15 250 00 00                  : Micro-inverse
HVOL .F3          70 000 00 00                  : Hole Volume
AVOL .F3          70 000 00 00                  : Annular Volume
DCOR .G/C3        42 356 01 00                  : Density Correction
DEN  .G/C3        42 350 01 00                  : Compensated Density
CTAO .MMHO        07 120 44 00                  : Array Ind. One Cond Ct
SPCG .MV          30 010 00 00                  : Spontaneous Potential
FEFE .OHMM        10 220 06 00                  : Shallow FE
RTAO .OHMM        07 120 44 00                  : Array Ind. One Res Rt
R40O .OHMM        07 120 44 00                  : Array Ind. One Res 40
R60O .OHMM        07 120 44 00                  : Array Ind. One Res 60
MLTC .IN          15 280 01 00                  : MML Caliper
TR21 .US          60 520 30 00                  : 3' Transit Time
TR11 .US          60 520 04 00                  : 4' Transit Time
TR22 .US          60 520 50 00                  : 5' Transit Time
TR12 .US          60 520 60 00                  : 6' Transit Time
DT35 .US/F        60 520 32 00                  : 3-5' Compensated Sonic
SPRL .PERC        61 890 20 00                  : Wyllie Lime. Sonic Por.
NPOR .PERC        42 890 04 00                  : Base Neutron Porosity
NPRS .PERC        42 890 03 00                  : Sandstone Neutron Por.
NPRD .PERC        42 890 02 00                  : Dolomite Neutron Por.
DPOR .PERC        45 890 13 00                  : Base Density Porosity
DPRS .PERC        45 890 12 00                  : Sandstone Density Por.
DPRD .PERC        45 890 11 00                  : Dolomite Density Por.
BIT  .IN          70 282 00 00                  : Bit size
#
~Parameter Information
#MNEM.UNIT        VALUE                         : DESCRIPTION
#----.----        -----                         : -----------
RNID .            5 INCH MAIN                   : Run ID
SON  .            3539922                       : Job Number
DREF .            KB                            : Depth reference
EREF .F              3400.000                   : Elevation of depth reference
GL   .F              3389.000                   : Ground elevation
RUN  .            ONE                           : Run number
TDD  .F              5721.000                   : TD (driller)
TDL  .F              5722.000                   : TD (logger)
BS   .IN                7.875                   : Bit size
TIMC .                                          : Time circulation ended
TIML .                                          : Time logger at bottom
UNIT .              13244                       : Equipment ID
BASE .            LIB                           : Equipment base location
ENG  .            ADAM SILL                     : Recording engineer
WIT  .            ROGER PEARSON                 : Witnessed by
CSGT .            SURFACE                       : Casing type
CSGS .IN                8.625                   : Casing size
CSGW .LB/FT            24.003                   : Casing weight
CSGD .F              1656.000                   : Casing bottom (driller)
CSGL .F              1659.000                   : Casing bottom (logger)
CSUP .F           -999999.000                   : Casing Stick Up GL
MUD  .            CHEMICAL                      : Mud type
MUDS .            FLOWLINE                      : Mud sample source
MUDD .G/C3              9.200                   : Mud density
MUDV .CP               54.000                   : Mud viscosity
PH   .                 10.500                   : Mud pH
FL   .ML/30MIN          8.800                   : Mud fluid loss rate
RM   .OHMM              1.280                   : Mud resistivity
RMT  .DEGF             93.900                   : Mud sample temperature
RMF  .OHMM              1.020                   : Mud filtrate resistivity
RMFT .DEGF             93.900                   : Mud filtrate sample temperature
RMC  .OHMM              1.540                   : Mud cake resistivity
RMCT .DEGF             93.900                   : Mud cake sample temperature
TMAX .DEGF            126.000                   : Max recorded temperature
#
~Other Information
--------------------------------------------------------------------------------------
- SOFTWARE ISSUE:  WLS 13.05.9583
- TOOLS: MCG, MML, MDN, MPD, MFE, MSS, MAI
- HARDWARE:
                           - MDN: DUAL BOWSPRING ECCENTRALIZER
                           - MFE: 1 X 0.5 INCH STANDOFF
                           - MSS: 2 X 0.5 INCH STANDOFF
                           - MAI: 2 X 0.5 INCH STANDOFF
- 2.71 G/CC LIMESTONE DENSITY MATRIX USED TO CALCULATE POROSITY
- BOREHOLE RUGOSITY, TIGHT PULLS, AND WASHOUTS WILL AFFECT DATA QUALITY
- ALL INTERVALS LOGGED AND SCALED PER CUSTOMER'S REQUEST
- TOTAL HOLE VOLUME FROM TD TO SURFACE CASING: 2200 CU.FT.
- ANNULAR HOLE VOLUME WITH 5.5 INCH PRODUCTION CASING FROM TD TO 3500 FEET: 460 CU.FT.
- RIG: MURFIN #21
- ENGINEER: ADAM SILL, DEREK CARTER
- OPERATOR: NICOLAS ADAME
--------------------------------------------------------------------------------------
#
#          DEPT
#          CGXT           SMTU           GRGC           NPRL           CLDC
#          PDPE           DPRL           MNRL           MINV           HVOL
#          AVOL           DCOR           DEN            CTAO           SPCG
#          FEFE           RTAO           R40O           R60O           MLTC
#          TR21           TR11           TR22           TR12           DT35
#          SPRL           NPOR           NPRS           NPRD           DPOR
#          DPRS           DPRD            BIT
#
~ASCII Log Data
       1649.000
     102.697389     858.476563      70.251253      37.418762       8.329126
      11.440724      39.975391       0.912983       1.046833       0.003515
       0.001982      -0.246071       2.026421       0.798621     169.291120
       0.081456    1252.159431    1252.159431    1252.159431       4.211846
     214.411837     269.416049     327.743125     383.546127      56.007859
       6.012622      37.737934      42.385889      34.096359      39.975391
      37.792676      45.111186    -999.250000
       1649.328
     102.612632     849.609375      65.786019      36.950882       8.328787
      11.383549      38.927418       0.908472       1.047653       0.003515
       0.001982      -0.243313       2.044341       0.795992     169.076411
       0.082104    1256.296260    1256.296260    1256.296260       4.211702
     214.289814     269.878800     327.856861     383.627303      56.004673
       6.010370      37.269464      41.984319      33.665488      38.927418
      36.706600      44.152882    -999.250000
       1649.656
     102.732923     849.609375      60.561741      36.386374       8.328720
      11.473748      38.526721       0.908126       1.045967       0.003515
       0.001982      -0.241760       2.051193       0.793151     168.720521
       0.082794    1260.796335    1260.796335    1260.796335       4.211193
     214.493753     269.564046     327.867317     383.633987      55.997005
       6.004951      36.704264      41.502619      33.145183      38.526721
      36.291328      43.786467    -999.250000

