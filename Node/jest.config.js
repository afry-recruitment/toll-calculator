export default {
  testEnvironment: "node",
  watchman: false,
  testMatch: ["<rootDir>/test/**/*.test.ts"],
  extensionsToTreatAsEsm: [".ts"],
  transform: {
    "^.+\\.ts$": "<rootDir>/jest.transform.cjs",
  },
  moduleNameMapper: {
    "^(\\.{1,2}/.*)\\.js$": "$1",
  },
};
