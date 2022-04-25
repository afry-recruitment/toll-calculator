export function isMultiplePassage(currentDate, latestPassingTime) {
  if (!currentDate || !latestPassingTime) return null;
  return (
    currentDate.getUTCHours() === latestPassingTime.getUTCHours() &&
    currentDate.getUTCDay() === latestPassingTime.getUTCDay()
  );
}
