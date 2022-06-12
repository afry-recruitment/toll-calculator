package linus.kjellgren.tollcalculator;

import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.stage.Stage;

import java.io.IOException;
import java.time.Instant;
import java.time.LocalDateTime;
import java.time.ZoneId;
import java.time.ZoneOffset;
import java.util.ArrayList;
import java.util.Date;
import javafx.beans.property.SimpleStringProperty;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.scene.control.TableColumn;
import javafx.scene.control.TableView;
import javafx.scene.control.cell.PropertyValueFactory;
import linus.kjellgren.tollcalculator.java.vehicles.RegestrationTollAndDate;

/**
 * JavaFX App
 */
public class App extends Application {

    private static Scene scene;
    private ArrayList<RegestrationTollAndDate> vehiclesPast = new ArrayList<>();
    private ObservableList<RegestrationTollAndDate> vehiclesTable = FXCollections.observableArrayList(vehiclesPast);

    @Override
    public void start(Stage stage) throws IOException {
        scene = new Scene(loadFXML("primary"), 800, 600);
        stage.setScene(scene);
        stage.show();
    }

    static void setRoot(String fxml) throws IOException {
        scene.setRoot(loadFXML(fxml));
    }

    private static Parent loadFXML(String fxml) throws IOException {
        FXMLLoader fxmlLoader = new FXMLLoader(App.class.getResource(fxml + ".fxml"));
        return fxmlLoader.load();
    }

    public static void main(String[] args) {
        launch();
    }
    
    
    public void initializeVehicles(){
        
    }
    
    
}