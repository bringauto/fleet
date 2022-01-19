module.exports = {
  root: true,
  env: {
    node: true,
  },
  extends: ["@notum-cz/eslint-config-notum-vue"],
  rules: {
    "no-console": process.env.NODE_ENV === "production" ? "warn" : "off",
    "no-debugger": process.env.NODE_ENV === "production" ? "warn" : "off",
    "spellcheck/spell-checker": "off",
    "import/prefer-default-export": "off",
    "import/named": "off",
  },
  overrides: [
    {
      files: ["**/__tests__/*.{j,t}s?(x)", "**/tests/unit/**/*.spec.{j,t}s?(x)"],
      env: {
        jest: true,
      },
    },
  ],
};
