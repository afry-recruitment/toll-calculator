import Mocha, { describe, before, it } from 'mocha';
import { expect } from 'chai';

import {isWeekend, isSameMonth, isSameDay, isTollFreeDate } from '../../services/dateServices.js';

describe('dateServices',()=>{


describe('#isWeekend()',()=>{
    it('without arguments',()=>{
        expect(isWeekend()).to.equal(null)
    })
    it('with date(2013-05-01 00:00:01) argument',()=>{
        expect(isWeekend(new Date("2013-05-01 00:00:01"))).to.equal(false)
    })
    it('with date(2013-05-04 00:00:01) argument',()=>{
        expect(isWeekend(new Date("2013-05-04 00:00:01"))).to.equal(true)
    })
})

describe('#isSameMonth()',()=>{
    it('without arguments',()=>{
        expect(isSameMonth()).to.equal(null)
    })
    it('with (new Date("2013-05-05 00:00:01"),new Date("2013-07-07 00:00:01") argument',()=>{
        expect(isSameMonth(new Date("2013-05-05 00:00:01"),new Date("2013-07-07 00:00:01"))).to.equal(false)
    })
    it('with (new Date("2013-05-01 00:00:01"),new Date("2013-05-11 00:00:01") argument',()=>{
        expect(isSameMonth(new Date("2013-05-01 00:00:01"),new Date("2013-05-11 00:00:01"))).to.equal(true)
    })
})


describe('#isSameDay()',()=>{
    it('without arguments',()=>{
        expect(isSameDay()).to.equal(null)
    })
    it('with (new Date("2013-05-05 00:00:01"),new Date("2013-07-07 00:00:01") argument',()=>{
        expect(isSameDay(new Date("2013-05-05 00:00:01"),new Date("2013-07-07 00:00:01"))).to.equal(false)
    })
    it('with (new Date("2013-05-01 00:00:01"),new Date("2013-05-01 00:00:01") argument',()=>{
        expect(isSameDay(new Date("2013-05-01 00:00:01"),new Date("2013-05-01 00:00:01"))).to.equal(true)
    })
})


describe('#isTollFreeDate()',()=>{
    it('without arguments',()=>{
        expect(isTollFreeDate()).to.equal(null)
    })
    it('with date(2013-05-01 00:00:01) argument',()=>{
        expect(isTollFreeDate(new Date("2013-05-02 00:00:01"))).to.equal(false)
    })
    it('with date(2013-05-04 00:00:01) argument',()=>{
        expect(isTollFreeDate(new Date("2013-05-04 00:00:01"))).to.equal(true)
    })
})


});
