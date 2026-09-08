const esbuild = require(
  require.resolve("esbuild", { paths: [require.resolve("tsx")] }),
);

module.exports = {
  process(sourceText, sourcePath) {
    return {
      code: esbuild.transformSync(sourceText, {
        format: "esm",
        loader: "ts",
        sourcefile: sourcePath,
        sourcemap: "inline",
        target: "es2022",
      }).code,
    };
  },
};
