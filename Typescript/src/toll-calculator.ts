import * as readline from 'readline';
import { TollCalculatorService } from './services/toll-calculator.service';
import { Vehicle } from './entities/vehicle.entity';
import { VehicleType } from './enums/vehicle.enum';
import * as chalk from 'chalk';

// the cli interface
const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout,
});

const service = new TollCalculatorService();

// visual header for the cli
const displayHeader = () => {
  console.clear();
  console.log(chalk.green('=================================='));
  console.log(chalk.green('=       TOLL FEE CALCULATOR      ='));
  console.log(chalk.green('=================================='));
  console.log('');
};

// get vehicle type from user
const getVehicleType = (): Promise<VehicleType> => {
  return new Promise((resolve) => {
    displayHeader();
    rl.question(
      chalk.blue('Enter vehicle type (e.g., Car, MotorBike): '),
      (type) => {
        try {
          const validVehicleType = validateVehicleType(type);
          resolve(validVehicleType);
        } catch (error) {
          console.error(chalk.red(error.message));
          return getVehicleType();
        }
      },
    );
  });
};

// get dates from user
const getDates = (): Promise<Date[]> => {
  return new Promise((resolve) => {
    displayHeader();
    rl.question(
      chalk.blue(
        'Enter dates (comma separated in format YYYY-MM-DDTHH:mm:ss): ',
      ),
      (datesStr) => {
        const dates = datesStr.split(',').map((date) => new Date(date.trim()));
        resolve(dates);
      },
    );
  });
};

// validate vehicle type
function validateVehicleType(value: string): VehicleType {
  if (Object.values(VehicleType).includes(value as VehicleType)) {
    return value as VehicleType;
  }
  throw new Error('Invalid vehicle type provided');
}

// main function for the cli
const main = async () => {
  try {
    const vehicleType = await getVehicleType();
    const dates = await getDates();
    const vehicle = new Vehicle(vehicleType);

    const fee = service.getTotalTollFee(dates, vehicle);

    displayHeader();
    console.log(
      chalk.green(
        `Total toll fee for ${vehicleType} on the provided dates is: ${fee}`,
      ),
    );
    console.log(
      chalk.yellow('Press any key to calculate again or Ctrl+C to exit.'),
    );
    rl.question('', () => main());
  } catch (error) {
    console.error('Error:', error.message);
  }
};

main();
