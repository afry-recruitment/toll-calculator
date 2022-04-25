import Mocha, { describe, before, it } from 'mocha';
import { expect } from 'chai';

import { calculateTollFee } from '../../services/calculateTollFeeServices.js';
import Car from '../../models/vehicles/Car.js';



describe('calculateTollFeeServices',()=>{


    describe('#calculateTollFee()',()=>{
        it('without arguments',()=>{
            expect(calculateTollFee()).to.equal(null)
        })
        it('with new Car(),date(2013-01-04T16:12:00) argument',()=>{
            expect(calculateTollFee(new Car(),new Date('2013-01-04T16:12:00'))).to.equal(8)
        })
        it('with new Car(),date(2013-01-04T17:19:00) argument',()=>{
            expect(calculateTollFee(new Car(), new Date('2013-01-04T17:19:00'))).to.equal(8)
        })
        it('with new Car(),date(2013-01-04T22:09:00) argument',()=>{
            expect(calculateTollFee(new Car(),new Date('2013-01-04T22:09:00'))).to.equal(0)
        })
        it('with new Car(),date(2013-01-04T09:09:00) argument',()=>{
            expect(calculateTollFee(new Car(),new Date('2013-01-04T10:09:00'))).to.equal(13)
        })
        it('with argument',()=>{
            expect(calculateTollFee(new Car(),
            new Date('2013-01-03T15:10:00Z'),// 13
            new Date('2013-01-03T15:20:00Z'),// 13
            new Date('2013-01-09T15:10:00Z'),// 13 +
            new Date('2013-01-03T15:40:00Z'),// 18 +

            new Date('2013-01-01T15:10:00Z'),// 0 Holiday
            new Date('2013-01-05T15:20:00Z'),// 0 Weekend
            new Date('2013-01-05T15:10:00Z'),// 0 Weekend
            new Date('2013-01-05T15:40:00Z'),// 0 Weekend

            new Date('2013-01-04T15:10:00Z'),// 13
            new Date('2013-01-04T15:20:00Z'),// 13
            new Date('2013-01-04T15:10:00Z'),// 13
            new Date('2013-01-04T15:40:00Z'),// 18 +
            )).to.equal(49)
        })

        it('with passing in similar hour and day and different day argument',()=>{
            expect(calculateTollFee(new Car(),
            new Date('2013-01-03T15:10:00Z'),// 13
            new Date('2013-01-03T15:20:00Z'),// 13
            new Date('2013-01-09T15:10:00Z'),// 13 +
            new Date('2013-01-03T15:40:00Z'),// 18 +
            )).to.equal(31)
        })


        it('with more than 60 argument',()=>{
            expect(calculateTollFee(new Car(),
            new Date('2013-01-03T15:10:00Z'),// 13
            new Date('2013-01-03T15:20:00Z'),// 13
            new Date('2013-01-09T15:10:00Z'),// 13 +
            new Date('2013-01-03T15:40:00Z'),// 18 +

            new Date('2013-01-01T15:10:00Z'),// 0 Holiday
            new Date('2013-01-05T15:20:00Z'),// 0 Weekend
            new Date('2013-01-05T15:10:00Z'),// 0 Weekend
            new Date('2013-01-05T15:40:00Z'),// 0 Weekend

            new Date('2013-01-04T15:10:00Z'),// 13
            new Date('2013-01-04T15:20:00Z'),// 13
            new Date('2013-01-04T15:10:00Z'),// 13
            new Date('2013-01-04T15:40:00Z'),// 18 +

            new Date('2013-01-15T15:10:00Z'),// 13
            new Date('2013-01-15T15:20:00Z'),// 13
            new Date('2013-01-15T15:10:00Z'),// 13 +
            new Date('2013-01-16T15:40:00Z'),// 18 +
            )).to.equal(60)
        })
    })
    
})    