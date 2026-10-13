class TollCalculator {
    getTollFee(vehicle, dates) {
        if (!vehicle) {
            throw new Error();
        }
        
        if (!vehicle instanceof Vehicle) {
            throw new Error();
        }

        if (!dates) {
            throw new Error();
        }

        if (this.#isTollFreeVehicle(vehicle)) {
            return 0;
        }

        if (dates.length === 0) {
            return 0;
        }

        const sortedDates = dates.sort((a, b) => a - b);

        let total = 0;
        let day = [];
        let curr = sortedDates[0];
        if (curr.getFullYear() !== 2013) {
            throw new Error();
        }
        day.push(curr);

        for (let i = 0; i < sortedDates.length; i++) {
            const next = sortedDates[i];
            if (next.getFullYear() !== 2013) {
                throw new Error();
            }

            if (this.#getDayOfYear(next) === this.#getDayOfYear(curr)) {
                day.push(next);
            } else {
                total += this.#getFeeForDay(day);
                day = [];
                curr = sortedDates[i];
                if (curr.getFullYear() !== 2013) {
                    throw new Error();
                }
                day.push(curr);
            }
        }

        total += this.#getFeeForDay(day);
        return total;
    }

    #isTollFreeVehicle(vehicle) {
        if (vehicle instanceof Diplomat) return true;
        else if (vehicle instanceof Emergency) return true;
        else if (vehicle instanceof Foreign) return true;
        else if (vehicle instanceof Military) return true;
        else if (vehicle instanceof Motorbike) return true;
        else if (vehicle instanceof Tractor) return true;
        else return false;
    }

    #getFeeForDay(day) {
        if (!day) {
            throw new Error();
        }

        if (day.length === 0) {
            return 0;
        }

        if (this.#isTollFreeDay(day[0])) {
            return 0;
        }

        if (day.length === 1) {
            return this.#getTollFeeByTime(day[0]);
        }

        let total = 0;
        let hour = [];

        let curr = day[0];
        hour.push(curr);
        let currHours = curr.getHours();
        let limit = new Date(curr);
        limit.setHours(currHours + 1);

        for (let i = 1; i < day.length; i++) {
            const next = day[i];
            if (next < limit) {
                hour.push(next);
            } else {
                total += this.#getHighestFeeInHour(hour);
                hour = [];
                curr = day[i];
                hour.push(curr);
                currHours = curr.getHours();
                limit = new Date(curr);
                limit.setHours(currHours + 1);
            }
        }

        total += this.#getHighestFeeInHour(hour);

        const maximumFee = 60;
        if (total > maximumFee) {
            total = maximumFee;
        }

        return total;
    }

    #getDayOfYear(date) {
        return Math.floor((date - new Date(date.getFullYear(), 0, 0)) / 1000 / 60 / 60 / 24);
    }

    #isTollFreeDay(date) {
        const dayOfYear = this.#getDayOfYear(date);

        const tollFreeDays = [
            1, 5, 6, 12, 13, 19, 20, 26, 27, 33, 34, 40, 41, 47, 48, 54, 55, 61, 62, 68, 69,
            75, 76, 82, 83, 87, 88, 89, 90, 91, 96, 97, 103, 104, 110, 111, 117, 118, 120,
            121, 124, 125, 128, 129, 131, 132, 138, 139, 145, 146, 152, 153, 156, 157, 159,
            160, 166, 167, 172, 173, 174, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189,
            190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205,
            206, 207, 208, 209, 210, 211, 212, 215, 216, 222, 223, 229, 230, 236, 237, 243,
            244, 250, 251, 257, 258, 264, 265, 271, 272, 278, 279, 285, 286, 292, 293, 299,
            300, 305, 306, 307, 313, 314, 320, 321, 327, 328, 334, 335, 341, 342, 348, 349,
            355, 356, 358, 359, 360, 362, 363, 365
        ];

        return tollFreeDays.includes(dayOfYear);
    }
    
    #getHighestFeeInHour(hour) {
        if (!hour) {
            throw new Error();
        }

        if (hour.length === 0) {
            return 0;
        }

        if (hour.length === 1) {
            return this.#getTollFeeByTime(hour[0]);
        }

        let highestFee = 0;
        for (let i = 0; i < hour.length; i++) {
            const fee = this.#getTollFeeByTime(hour[i]);
            if (fee > highestFee) {
                highestFee = fee;
            }
        }

        return highestFee;
    }

    #getTollFeeByTime(date) {
        const minute = (date.getHours() * 60) + date.getMinutes();

        const t_0600 = 6 * 60;
        const t_0629 = (6 * 60) + 29;
        const t_0630 = (6 * 60) + 30;
        const t_0659 = (6 * 60) + 59;
        const t_0700 = 7 * 60;
        const t_0759 = (7 * 60) + 59;
        const t_0800 = 8 * 60;
        const t_0829 = (8 * 60) + 29;
        const t_0830 = (8 * 60) + 30;
        const t_1459 = (14 * 60) + 59;
        const t_1500 = 15 * 60;
        const t_1529 = (15 * 60) + 29;
        const t_1530 = (15 * 60) + 30;
        const t_1659 = (16 * 60) + 59;
        const t_1700 = 17 * 60;
        const t_1759 = (17 * 60) + 59;
        const t_1800 = 18 * 60;
        const t_1829 = (18 * 60) + 29;
        
        if (minute >= t_0600 && minute <= t_0629)
        {
            return 8;
        }

        if (minute >= t_0630 && minute <= t_0659)
        {
            return 13;
        }
        if (minute >= t_0700 && minute <= t_0759)
        {
            return 18;
        }

        if (minute >= t_0800 && minute <= t_0829)
        {
            return 13;
        }

        if (minute >= t_0830 && minute <= t_1459)
        {
            return 8;
        }

        if (minute >= t_1500 && minute <= t_1529)
        {
            return 13;
        }

        if (minute >= t_1530 && minute <= t_1659)
        {
            return 18;
        }

        if (minute >= t_1700 && minute <= t_1759)
        {
            return 13;
        }

        if (minute >= t_1800 && minute <= t_1829)
        {
            return 8;
        }

        return 0;
    }
}

class Vehicle {
  constructor(vehicleType) {
    this.vehicleType = vehicleType;
  }

  getVehicleType() {
    return `${this.vehicleType}`;
  }
}

class Car extends Vehicle {
    constructor() {
        super("Car");
    }
}

class Diplomat extends Vehicle {
    constructor() {
        super("Diplomat");
    }
}

class Emergency extends Vehicle {
    constructor() {
        super("Emergency");
    }
}

class Foreign extends Vehicle {
    constructor() {
        super("Foreign");
    }
}

class Military extends Vehicle {
    constructor() {
        super("Military");
    }
}

class Motorbike extends Vehicle {
    constructor() {
        super("Motorbike");
    }
}

class Tractor extends Vehicle {
    constructor() {
        super("Tractor");
    }
}

const car = new Car();
// 2013-10-09...
const dates = [
    new Date(2013, 9, 9, 6, 30, 0, 0),
    new Date(2013, 9, 9, 7, 30, 0, 0),
    new Date(2013, 9, 9, 8, 30, 0, 0),
    new Date(2013, 9, 9, 9, 30, 0, 0),
    new Date(2013, 9, 9, 10, 30, 0, 0),
    new Date(2013, 9, 9, 11, 30, 0, 0)
];

const tollCalculator = new TollCalculator();
const result = tollCalculator.getTollFee(car, dates);
console.log(result);
