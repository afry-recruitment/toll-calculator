       IDENTIFICATION DIVISION.
       PROGRAM-ID. TOLLCALCULATOR.
       AUTHOR.     ANUDEEPA ALAMPADATH.
      *****************************************************************
      * DATE        :  2025.04.25
      * DESCRIPTION :  TOLL FEE CALCULATOR 1.0
      *                A CALCULATOR FOR VEHICLE TOLL FEES.
      *              * Fees will differ between 8 SEK and 18 SEK,
      *                depending on the time of day 
      *              * Rush-hour traffic will render the highest fee
      *              * The maximum fee for one day is 60 SEK
      *              * A vehicle should only be charged once an HOUR
      *              * In the case of multiple fees in the same 
      *                HOUR period, the highest one applies.
      *              * Some vehicle types are fee-free
      *              * Weekends and holidays are fee-free
      *****************************************************************

       ENVIRONMENT DIVISION.
       CONFIGURATION SECTION.
       INPUT-OUTPUT SECTION.
       FILE-CONTROL.

       DATA DIVISION.
       FILE SECTION.
      ******************************************************************
       WORKING-STORAGE SECTION.

       01  WS-VEHICLE-TYPE           PIC X(10).
       01  WS-DATE-ENTRY             PIC X(8).
       01  WS-HOLIDAY-FLAG           PIC X VALUE SPACE.
       01  WS-TIME-ENTRY             PIC X(05).
       01  WS-TOTAL-FEE              PIC 9(03) VALUE 0.
       01  WS-CURRENT-FEE            PIC 9(03).
       01  WS-MAX-DAILY-FEE          PIC 9(03) VALUE 60.

       01  WS-CURRENT-HOUR          PIC 9(02) VALUE 99.
      
       01  WS-EXEMPT-VEH-FLAG        PIC X VALUE 'N'.

       01  VEHICLE-EXEMPT-LIST.
           05  WS-EXEMPT-VEHICLE OCCURS 6 TIMES.
               10  EXEMPT-VEH-TYP     PIC X(10).

       01  HOLIDAY-DATE-LIST.
           05  HOLIDAY-DATE OCCURS 10 TIMES.
               10  HOLIDAY-VALUE     PIC X(10).


       01  IDX                       PIC 9(02) VALUE 1.
       01  WS-HOUR                   PIC 9(02).
       01  WS-MINUTE                 PIC 9(02).
       01  TIME-HH-STR               PIC X(02).
       01  TIME-MM-STR               PIC X(02).
       01  TIME-LEN                  PIC 9(01).
       01  WS-HOURLY-MAXFEE          PIC 9(03) VALUE 0.
       01 TIME-IDX                   PIC 9(02).
       01 TIME-COUNT                 PIC 9(02) VALUE 0.
       
       01 WS-TIME-ENTRY-TABLE.
          05 WS-TIME-ENTRY-ITEM OCCURS 10 TIMES.
               10 TIME-VAL           PIC X(05).
          
       01 TIME-VAL-BUFFER            PIC X(05).

      ******************************************************************
            PROCEDURE DIVISION.
      ******************************************************************

           DISPLAY "ENTER TYPE OF VEHICLE (e.g., CAR, TRACTOR): "
           ACCEPT   WS-VEHICLE-TYPE

           DISPLAY "ENTER DATE (YYYYMMDD): "
           ACCEPT   WS-DATE-ENTRY
      
      *    ENTER TIMES TO CHECK

           PERFORM VARYING TIME-IDX FROM 1 BY 1 UNTIL TIME-IDX > 10
               DISPLAY "ENTER TIME (HH:MM) OR STOP:"
               ACCEPT   TIME-VAL-BUFFER
                        IF TIME-VAL-BUFFER = "STOP"
                           EXIT PERFORM
                        END-IF
               ADD 1 TO TIME-COUNT
               MOVE TIME-VAL-BUFFER TO WS-TIME-ENTRY-ITEM(TIME-IDX)
           END-PERFORM
           
           PERFORM EXEMPT-VEHICLE-LIST
           PERFORM CHECK-DATE-HOLIDAY
           PERFORM CHECK-EXEMPTION
           PERFORM TOLL-FEE-CALCULATION.
      
      *    LIST OF VEHICLES EXEMPTED 

       EXEMPT-VEHICLE-LIST.

           MOVE "MOTORBIKE" TO EXEMPT-VEH-TYP(1)
           MOVE "TRACTOR"   TO EXEMPT-VEH-TYP(2)
           MOVE "EMERGENCY" TO EXEMPT-VEH-TYP(3)
           MOVE "DIPLOMAT"  TO EXEMPT-VEH-TYP(4)
           MOVE "FOREIGN"   TO EXEMPT-VEH-TYP(5)
           MOVE "MILITARY"  TO EXEMPT-VEH-TYP(6).
      
      *    CHECK IF ENTERED DATE IS A HOLIDAY
       CHECK-DATE-HOLIDAY.

           CALL 'HOLIDAYCHECK' USING WS-DATE-ENTRY WS-HOLIDAY-FLAG
                IF WS-HOLIDAY-FLAG = 'Y'
                   DISPLAY "IT IS A HOLIDAY - NO TOLL APPLIED!."
                   STOP RUN
                END-IF.
      
      *    CHECKING WHETHER VEHICLE IS EXEMPTED
        CHECK-EXEMPTION.
      
           PERFORM VARYING IDX FROM 1 BY 1 UNTIL IDX > 6
               IF WS-VEHICLE-TYPE = EXEMPT-VEH-TYP(IDX)
                   MOVE 'Y' TO WS-EXEMPT-VEH-FLAG
                   DISPLAY "NO TOLL - VEHICLE EXEMPTED!"
                   STOP RUN
               END-IF
           END-PERFORM.
           
       TOLL-FEE-CALCULATION.
       
           MOVE 0  TO WS-TOTAL-FEE
           MOVE 0  TO WS-HOURLY-MAXFEE
           MOVE 99 TO WS-CURRENT-HOUR
       
       PERFORM VARYING TIME-IDX FROM 1 BY 1 UNTIL TIME-IDX > TIME-COUNT
           MOVE WS-TIME-ENTRY-ITEM(TIME-IDX) TO WS-TIME-ENTRY
           PERFORM CALCULATE-FEE
       
           IF WS-HOUR NOT = WS-CURRENT-HOUR
               ADD WS-HOURLY-MAXFEE    TO WS-TOTAL-FEE
               MOVE WS-HOUR            TO WS-CURRENT-HOUR
               MOVE WS-CURRENT-FEE     TO WS-HOURLY-MAXFEE
           ELSE
               IF WS-CURRENT-FEE > WS-HOURLY-MAXFEE
                   MOVE WS-CURRENT-FEE TO WS-HOURLY-MAXFEE
               END-IF
           END-IF

       END-PERFORM

      *    MAXIMUM HOURLY FEE
           ADD      WS-HOURLY-MAXFEE   TO WS-TOTAL-FEE
           DISPLAY  "MAX-HOURLY FEE:"     WS-TOTAL-FEE
           
      *    MAXIMUM FEE FOR ONE DAY MUST BE 60 SEK
           IF WS-TOTAL-FEE > WS-MAX-DAILY-FEE
               MOVE WS-MAX-DAILY-FEE   TO WS-TOTAL-FEE 
               DISPLAY "MAX DAILY FEE:"   WS-TOTAL-FEE              
           END-IF.
       
       CALCULATE-FEE.

           UNSTRING WS-TIME-ENTRY DELIMITED BY ":"
               INTO TIME-HH-STR TIME-MM-STR
           END-UNSTRING

           INSPECT TIME-HH-STR REPLACING ALL SPACE BY ZERO
           INSPECT TIME-MM-STR REPLACING ALL SPACE BY ZERO

           MOVE FUNCTION NUMVAL(TIME-HH-STR) TO WS-HOUR
           MOVE FUNCTION NUMVAL(TIME-MM-STR) TO WS-MINUTE

           EVALUATE TRUE
               WHEN WS-HOUR = 6 AND WS-MINUTE >= 30 AND WS-MINUTE <= 59
                   MOVE 13  TO  WS-CURRENT-FEE
               WHEN WS-HOUR = 6 AND WS-MINUTE >= 0 AND WS-MINUTE <= 29
                   MOVE 8   TO  WS-CURRENT-FEE
               WHEN WS-HOUR = 7
                   MOVE 18  TO  WS-CURRENT-FEE
               WHEN WS-HOUR = 8 AND WS-MINUTE <= 29
                   MOVE 13  TO  WS-CURRENT-FEE
               WHEN WS-HOUR >= 8 AND WS-HOUR<=14
                    AND WS-MINUTE >= 30 AND WS-MINUTE<= 59
                   MOVE 8   TO  WS-CURRENT-FEE
               WHEN WS-HOUR = 15 AND WS-MINUTE <= 29
                   MOVE 13  TO  WS-CURRENT-FEE
               WHEN WS-HOUR = 15 AND WS-MINUTE >= 30
                   MOVE 18  TO  WS-CURRENT-FEE
               WHEN WS-HOUR = 16
                   MOVE 18  TO  WS-CURRENT-FEE
               WHEN WS-HOUR = 17
                   MOVE 13  TO  WS-CURRENT-FEE
               WHEN WS-HOUR = 18 AND WS-MINUTE <= 29
                   MOVE 8   TO  WS-CURRENT-FEE
               WHEN OTHER
                   MOVE 0   TO  WS-CURRENT-FEE
           END-EVALUATE.

       EXIT.
