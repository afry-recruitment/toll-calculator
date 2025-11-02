// @ts-check
import { test, expect } from '@playwright/test';

test('has title', async ({ page }) => {
  // when the page is opened
  await page.goto('http://localhost:3000/');

  // then the page title should be correct
  await expect(page).toHaveTitle(/Toll Fee Calculator/);
});

test('selects vehicle type', async ({ page }) => {
  // when the page is opened
  await page.goto('http://localhost:3000/');

  // then the vehicle types should be loaded
  await expect(page.getByText('Please, wait until the vehicle types are loading!')).toHaveCount(0);

  // when the vehicle type dropdown list is opened 
  await page.locator('#vehicle-type-selection-select').click();

  // then all the different values should be visible
  await expect(page.getByText('Car')).toHaveCount(1);
  await expect(page.getByText('Diplomat')).toHaveCount(1);
  await expect(page.getByText('Emergency')).toHaveCount(1);
  await expect(page.getByText('Foreign')).toHaveCount(1);
  await expect(page.getByText('Military')).toHaveCount(1);
  await expect(page.getByText('Motorbike')).toHaveCount(1);
  await expect(page.getByText('Tractor')).toHaveCount(1);

  // when the Car vehicle type is selected
  await page.getByText('Car').click()

  // then the other vehicle types should be visible anymore
  await expect(page.getByText('Car')).toHaveCount(1);
  await expect(page.getByText('Diplomat')).toHaveCount(0);
  await expect(page.getByText('Emergency')).toHaveCount(0);
  await expect(page.getByText('Foreign')).toHaveCount(0);
  await expect(page.getByText('Military')).toHaveCount(0);
  await expect(page.getByText('Motorbike')).toHaveCount(0);
  await expect(page.getByText('Tractor')).toHaveCount(0);
});
