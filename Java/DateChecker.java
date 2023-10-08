import java.time.*;
import java.util.HashSet;
import java.util.Set;

public class DateChecker {


        // Define the list dates using LocalDate and Year, month, day
        public static Set<LocalDate> TOLL_FREE_DAYS = new HashSet<>();

        static {
            // Populate with toll-free dates
            TOLL_FREE_DAYS.add(LocalDate.of(2023, 1, 1));
            TOLL_FREE_DAYS.add(LocalDate.of(2023, 4, 8));
            TOLL_FREE_DAYS.add(LocalDate.of(2023, 4, 9));
            TOLL_FREE_DAYS.add(LocalDate.of(2023, 4, 10));
            TOLL_FREE_DAYS.add(LocalDate.of(2023, 5, 1));
            TOLL_FREE_DAYS.add(LocalDate.of(2023, 6, 6));
            TOLL_FREE_DAYS.add(LocalDate.of(2023, 11, 4));
            TOLL_FREE_DAYS.add(LocalDate.of(2023, 12, 24));
            TOLL_FREE_DAYS.add(LocalDate.of(2023, 12, 25));
            TOLL_FREE_DAYS.add(LocalDate.of(2023, 12, 26));
            TOLL_FREE_DAYS.add(LocalDate.of(2023, 12, 31));
        }


        public static boolean isTollFree(LocalDate date) {
            return isWeekend(date) || isTollFreeDay(date);
        }

        private static boolean isWeekend(LocalDate date) {
            return date.getDayOfWeek() == DayOfWeek.SATURDAY || date.getDayOfWeek() == DayOfWeek.SUNDAY;
        }

        // check if the date is matched in the toll-free-days list above
        private static boolean isTollFreeDay(LocalDate date) {
            return TOLL_FREE_DAYS.contains(date);
        }
    }

