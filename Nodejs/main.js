import Car from "./models/vehicles/Car.js";
const CarInstance = new Car();
console.log(CarInstance.isTollFree())



import dateServices from "./services/dateServices.js";
const dateServicesInstance = new dateServices()
console.log("111",await dateServicesInstance.getHolidays(2022,"SE"));
console.log("222",await dateServicesInstance.getHolidays(2022,"SE"));
console.log("444",await dateServicesInstance.getHolidays(2022,"SE"));