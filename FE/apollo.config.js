module.exports = {
  client: {
    service: {
      name: "fleet",
      // URL to the GraphQL API
      url: process.env.APP_VUE_GRAPHQL_URL,
    },
    // Files processed by the extension
    includes: ["src/**/*.vue", "src/**/*.js"],
  },
};
