module.exports = {
  client: {
    service: {
      name: "industrial-portal",
      // URL to the GraphQL API
      url: process.env.APP_VUE_GRAPHQL_URL,
    },
    // Files processed by the extension
    includes: ["src/**/*.vue", "src/**/*.js"],
  },
};
