import Mocha, { describe, before, it } from 'mocha';
import { expect } from 'chai';

import { isMultiplePassage } from '../../services/passingServices.js';

describe('passingServices', () => {
  describe('#isMultiplePassage()', () => {
    it('without arguments', () => {
      expect(isMultiplePassage()).to.equal(null);
    });
    it('with new date(2013-01-04T16:12:00),date(2013-01-04T16:12:00) argument', () => {
      expect(
        isMultiplePassage(
          new Date('2013-01-04T22:27:00'),
          new Date('2013-01-04T22:25:00')
        )
      ).to.equal(true);
    });
    it('with new date(2013-01-04T16:12:00),date(2013-01-04T16:12:00) argument', () => {
      expect(
        isMultiplePassage(
          new Date('2013-01-04T18:09:00'),
          new Date('2013-01-04T17:09:00')
        )
      ).to.equal(false);
    });
  });
});
