package linus.kjellgren.tollcalculator;

import java.io.IOException;
import java.net.URL;
import java.time.Instant;
import java.time.LocalDateTime;
import java.time.ZoneId;
import java.time.ZoneOffset;
import java.util.ArrayList;
import java.util.Date;
import java.util.ListIterator;
import java.util.ResourceBundle;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.Initializable;
import javafx.scene.control.Button;
import javafx.scene.control.ChoiceBox;
import javafx.scene.control.Label;
import javafx.scene.control.MenuItem;
import javafx.scene.control.TableColumn;
import javafx.scene.control.TableView;
import javafx.scene.control.TextField;
import javafx.scene.control.cell.PropertyValueFactory;
import linus.kjellgren.tollcalculator.java.vehicles.Car;
import linus.kjellgren.tollcalculator.java.vehicles.Diplomat;
import linus.kjellgren.tollcalculator.java.vehicles.Emergency;
import linus.kjellgren.tollcalculator.java.vehicles.Foreign;
import linus.kjellgren.tollcalculator.java.vehicles.Military;
import linus.kjellgren.tollcalculator.java.vehicles.Motorbike;
import linus.kjellgren.tollcalculator.java.vehicles.RegestrationTollAndDate;
import linus.kjellgren.tollcalculator.java.vehicles.Tractor;
import linus.kjellgren.tollcalculator.java.vehicles.Truck;
import linus.kjellgren.tollcalculator.java.vehicles.Vehicle;
import tollcallculator.TollCalculator;

public class PrimaryController implements Initializable{
    
    private final String[] vehicleTypeList = {"Car", "Truck", "Motorbike", "Diplomat", "Emergency", "Foreign", "Military", "Tractor"};
    private ObservableList<RegestrationTollAndDate> vehicleObservableList = FXCollections.observableArrayList();
    ZoneId ourZone = ZoneId.of("UTC+1");
    
    
    @FXML
    private TableView<RegestrationTollAndDate> tollTable;

    @FXML
    private TableColumn<RegestrationTollAndDate, String> regNrColumn;

    @FXML
    private TableColumn<RegestrationTollAndDate, Float> tollColumn;
    
    @FXML
    private TableColumn<RegestrationTollAndDate, String> datesColumn;
    @FXML
    private TableColumn<RegestrationTollAndDate, Vehicle> vehicleTypeColumn;
    
    @FXML
    private TextField newRegNr;
    
    @FXML
    private ChoiceBox<String> newVehicleType;
    
    @FXML
    private Button newToll;
    
    @Override
    public void initialize(URL url, ResourceBundle rb) {
        this.newVehicleType.setValue("Car");
        this.newVehicleType.getItems().addAll(vehicleTypeList);
        
        //Sets up the columns
        regNrColumn.setCellValueFactory(new PropertyValueFactory<RegestrationTollAndDate, String>("registration"));
        tollColumn.setCellValueFactory(new PropertyValueFactory<RegestrationTollAndDate, Float>("toll"));
        datesColumn.setCellValueFactory(new PropertyValueFactory<RegestrationTollAndDate, String>("datesString"));
        vehicleTypeColumn.setCellValueFactory(new PropertyValueFactory<RegestrationTollAndDate, Vehicle>("vehicleTypeString"));
        //load dummy data
        tollTable.setItems(getVehicles());
    }
    
    
    public ObservableList<RegestrationTollAndDate> getVehicles(){
        ArrayList<Date> dates = new ArrayList<>();
        LocalDateTime localDateTime = LocalDateTime.of(2022, 06, 10, 12, 00, 00);
        dates.add(localDateTimeToDate(localDateTime));
        localDateTime = LocalDateTime.of(2022, 06, 10, 12, 30, 00);
        dates.add(localDateTimeToDate(localDateTime));
        localDateTime = LocalDateTime.of(2022, 06, 10, 13, 45, 00);
        dates.add(localDateTimeToDate(localDateTime));
        localDateTime = LocalDateTime.of(2022, 06, 10, 18, 30, 00);
        dates.add(localDateTimeToDate(localDateTime));
        vehicleObservableList.add(new RegestrationTollAndDate("ABC123", 26, new Car(), dates));
        dates = new ArrayList<>();
        localDateTime = LocalDateTime.of(2022, 06, 03, 13, 45, 00);
        dates.add(localDateTimeToDate(localDateTime));
        vehicleObservableList.add(new RegestrationTollAndDate("DEF456", 18, new Truck(), dates));
        dates = new ArrayList<>();
        localDateTime = LocalDateTime.of(2022, 06, 12, 10, 15, 00);
        dates.add(localDateTimeToDate(localDateTime));
        vehicleObservableList.add(new RegestrationTollAndDate("DEF456", 18, new Truck(), dates));
        return vehicleObservableList;
    }
    private Date localDateTimeToDate(LocalDateTime localDateTime){
        Instant instantFromDate = localDateTime.toInstant(ZoneOffset.UTC);
        return java.util.Date.from(instantFromDate);
    }
    
    
    @FXML
    void registerNewToll(ActionEvent event) {
        String regNr = newRegNr.getText();
        String vehicleType = this.newVehicleType.getValue();
        TollCalculator tollCalculator = new TollCalculator();
        boolean existed = false;
        if (regNr.isBlank() || vehicleType.isBlank()){
            return;
        }
        ListIterator<RegestrationTollAndDate> iterator = vehicleObservableList.listIterator();
        while (iterator.hasNext()){
            RegestrationTollAndDate iteration = iterator.next();
            if (iteration.getRegistration().equals(regNr)){
                LocalDateTime localDateTime = LocalDateTime.now(ourZone);
                int year = iteration.getDates().get(0).toInstant().atZone(ourZone).getYear();
                int month = iteration.getDates().get(0).toInstant().atZone(ourZone).getMonthValue();
                int day = iteration.getDates().get(0).toInstant().atZone(ourZone).getDayOfMonth();
                if (localDateTime.getYear()==year && localDateTime.getMonthValue()==month && localDateTime.getDayOfMonth()==day){
                    existed = true;
                    ArrayList<Date> dateList =  iteration.getDates();
                    dateList.add(localDateTimeToDate(localDateTime));
                    Date[] newDateList = dateList.toArray(new Date[0]);
                    Integer toll = tollCalculator.getTollFee(iteration.getVehicleType(), newDateList);
                    if (toll>iteration.getToll()){
                        iteration.setToll(toll);
                        iterator.set(iteration);
                    }
                }
            }
        }
        if (!existed){
            ArrayList<Date> dateList = new ArrayList<>();
            LocalDateTime localDateTime = LocalDateTime.now(ourZone);
            dateList.add(localDateTimeToDate(localDateTime));
            Vehicle vehicleObject;
            if(vehicleType.equals("Motorbike")){
                vehicleObject = new Motorbike();
            } else if(vehicleType.equals("Diplomat")){
                vehicleObject = new Diplomat();
            } else if(vehicleType.equals("Emergency")){
                vehicleObject = new Emergency();
            } else if(vehicleType.equals("Foreign")){
                vehicleObject = new Foreign();
            } else if(vehicleType.equals("Military")){
                vehicleObject = new Military();
            } else if(vehicleType.equals("Tractor")){
                vehicleObject = new Tractor();
            } else if(vehicleType.equals("Truck")){
                vehicleObject = new Truck();
            }else {
                vehicleObject = new Car();
            }
            Date[] dateArray = {localDateTimeToDate(localDateTime)};
            Integer toll = tollCalculator.getTollFee(vehicleObject, dateArray);
            RegestrationTollAndDate newToll = new RegestrationTollAndDate(regNr, toll, vehicleObject, dateList);
            vehicleObservableList.add(newToll);
        }
        tollTable.setItems(vehicleObservableList);
        tollTable.refresh();
    }
}
