export const getDate = (date: Date): { year: number; month: number; day: number } => {
  const year = date.getFullYear();
  const month = date.getMonth() + 1;
  const day = date.getDate();
  return { year, month, day };
};
export const getTime = (date: Date): { hour: number; minute: number } => {
  const hour = date.getHours();
  const minute = date.getMinutes();
  return { hour, minute };
};

export const isWithinOneHour = (start: Date, date: Date): boolean => {
  const elapsedMs = date.getTime() - start.getTime();

  const oneHourMs = 60 * 60 * 1000;

  return elapsedMs <= oneHourMs;
};

export const formatTime = (date: Date): string => {
  const hour = String(date.getHours()).padStart(2, "0");
  const minute = String(date.getMinutes()).padStart(2, "0");
  return `${hour}:${minute}`;
};
