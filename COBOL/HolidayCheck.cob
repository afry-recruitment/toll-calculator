       IDENTIFICATION DIVISION.
       PROGRAM-ID. HOLIDAYCHECK.
       AUTHOR.     ANUDEEPA ALAMPADATH.
      *****************************************************************
      * DATE        :  2025.04.25
      * DESCRIPTION :  PROGRAM TO CHECK GIVEN DATE IS A SATURDAY,
      *                SUNDAY OR NATIONAL HOLIDAY
      *                IF YES, HOLIDAY-FLAG IS MARKED & RETURNED AS YES
      *****************************************************************

       ENVIRONMENT DIVISION.
       CONFIGURATION SECTION.
       INPUT-OUTPUT SECTION.
       FILE-CONTROL.

       DATA DIVISION.
       FILE SECTION.
      ******************************************************************
       WORKING-STORAGE SECTION.

       01 WS-INP-DATE               PIC 9(8).
       01 WS-INP-DATE-FUNC          PIC 9(8).
       01 WS-DAY-OF-WEEK            PIC S9(8).
       01 WS-QUOTIENT               PIC S9(4).
       01 WS-REMAINDER              PIC S9(4).
       01 IDX                       PIC 9(2) VALUE 01.

       01 HOLIDAY-LIST.
           05 HOLIDAY-DATE OCCURS 10 TIMES.
              10 HOLIDAY-DATE-VALUE PIC X(10).

      ******************************************************************
       LINKAGE SECTION.
       01 LNK-PASSED-DATE          PIC X(8). 
       01 LNK-HOLIDAY-FLAG         PIC X.
      ******************************************************************

       PROCEDURE DIVISION USING LNK-PASSED-DATE LNK-HOLIDAY-FLAG.

           MOVE "20250101" TO HOLIDAY-DATE-VALUE(1)
           MOVE "20251225" TO HOLIDAY-DATE-VALUE(2)


           PERFORM VARYING IDX FROM 1 BY 1 UNTIL IDX > 10
                   IF  LNK-PASSED-DATE = HOLIDAY-DATE-VALUE(IDX)
                   MOVE 'Y' TO LNK-HOLIDAY-FLAG
                   GOBACK
                   END-IF
           END-PERFORM

           MOVE LNK-PASSED-DATE TO WS-INP-DATE

          COMPUTE WS-DAY-OF-WEEK = FUNCTION INTEGER-OF-DATE(WS-INP-DATE)                 
               DIVIDE WS-DAY-OF-WEEK  BY 7 GIVING  WS-QUOTIENT  
               REMAINDER WS-REMAINDER
                      
                      EVALUATE WS-REMAINDER
                      WHEN 0
                          MOVE 'Y' TO LNK-HOLIDAY-FLAG
                      WHEN 6                             
                          MOVE 'Y' TO LNK-HOLIDAY-FLAG              
                      END-EVALUATE

                      GOBACK.
           