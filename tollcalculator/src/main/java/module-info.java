module linus.kjellgren.tollcalculator {
    requires javafx.controls;
    requires javafx.fxml;

    opens linus.kjellgren.tollcalculator to javafx.fxml;
    opens linus.kjellgren.tollcalculator.java.vehicles to javafx.fxml;
    exports linus.kjellgren.tollcalculator;
    exports linus.kjellgren.tollcalculator.java.vehicles;
}
