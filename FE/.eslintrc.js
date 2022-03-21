module.exports = {
  root: true,
  env: {
    node: true,
  },
  extends: ["@notum-cz/eslint-config-notum-vue", "prettier"],
  rules: {
    "no-console": process.env.NODE_ENV === "production" ? "warn" : "off",
    "no-debugger": process.env.NODE_ENV === "production" ? "warn" : "off",
    "spellcheck/spell-checker": "off",
    "import/prefer-default-export": "off",
    "import/named": "off",
    "prettier/prettier": ["error", { endOfLine: "auto" }],
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
