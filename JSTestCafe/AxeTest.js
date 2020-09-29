import { axeCheck, createReport } from 'axe-testcafe';

fixture `TestCafe tests with Axe`
    .page `http://magenicautomation.azurewebsites.net/`;


test('Automated accessibility testing', async t => {
    const axeContext = { exclude: [['#iFrameButton']] };
    const axeOptions = { rules: { 'html-has-lang': { enabled: false } } };
    const { error, violations } = await axeCheck(t, axeContext, axeOptions);
    await t.expect(violations.length === 0).ok(createReport(violations));
});