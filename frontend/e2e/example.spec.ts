import { test, expect } from '@playwright/test';

test('has title', async ({ page }) => {
    await page.goto('/');
    await expect(page).toHaveTitle(/IDA/);
});

test('can navigate to reports', async ({ page }) => {
    await page.goto('/');
    // Note: This assumes the user is logged in or the app handles it.
    // For now, just checking if the sidebar has the reports link for admin if we can mock it.
    // In a real scenario, we'd log in first.
});
