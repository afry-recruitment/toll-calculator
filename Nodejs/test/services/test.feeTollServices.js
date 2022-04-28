import Mocha, { describe, before, it } from 'mocha';
import { expect, assert } from 'chai';

import {
  convertToTime,
  isBetweenIntervals,
  getPassingTime,
  getTollFee,
} from '../../services/feeTollServices.js';

describe('feeTollServices', () => {
  describe('#getTollFee()', () => {
    it('without arguments', () => {
      expect(getTollFee()).to.equal(null);
    });
    it('with 2013-01-04T17:09:00 must be 8 argument', () => {
      expect(getTollFee(new Date('2013-01-04T17:09:00'))).to.equal(8);
    });
    it('with 2013-01-04T07:19:00 must be 0 argument', () => {
      expect(getTollFee(new Date('2013-01-04T07:19:00'))).to.equal(0);
    });
    it('with 2013-01-04T21:19:00 must be 13 argument', () => {
      expect(getTollFee(new Date('2013-01-04T21:19:00'))).to.equal(13);
    });
  });

  describe('#getPassingTime()', () => {
    it('without arguments', () => {
      expect(getPassingTime()).to.equal(null);
    });
    it('with new Date(2013-01-05T10:02:00.000Z) argument', () => {
      assert.deepEqual(
        getPassingTime(new Date('2013-01-05T10:02:00.000Z')),
        new Date('2013-01-05T10:02:00.000Z')
      );
    });
  });

  describe('#convertToTime()', () => {
    it('without arguments', () => {
      expect(convertToTime()).to.equal(null);
    });
    it('with 10,2 argument', () => {
      assert.deepEqual(
        convertToTime(10, 2),
        new Date('2013-01-05T10:02:00.000Z')
      );
    });
  });

  describe('#isBetweenIntervals()', () => {
    it('without arguments', () => {
      expect(isBetweenIntervals()).to.equal(null);
    });
    let date = convertToTime(10, 2);
    let min = convertToTime(5, 2);
    let max = convertToTime(22, 2);
    it('with correct argument', () => {
      expect(isBetweenIntervals(date, min, max)).to.equal(true);
    });
    let date2 = convertToTime(5, 2);
    let min2 = convertToTime(20, 2);
    let max2 = convertToTime(22, 2);
    it('with incorrect argument', () => {
      expect(isBetweenIntervals(date2, min2, max2)).to.equal(false);
    });
  });

  describe('#isBetweenIntervals()', () => {
    it('without arguments', () => {
      expect(isBetweenIntervals()).to.equal(null);
    });
    let date = convertToTime(10, 2);
    let min = convertToTime(5, 2);
    let max = convertToTime(22, 2);
    it('with correct argument', () => {
      expect(isBetweenIntervals(date, min, max)).to.equal(true);
    });
    let date2 = convertToTime(5, 2);
    let min2 = convertToTime(20, 2);
    let max2 = convertToTime(22, 2);
    it('with incorrect argument', () => {
      expect(isBetweenIntervals(date2, min2, max2)).to.equal(false);
    });
  });
});
