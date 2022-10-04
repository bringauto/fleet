module.exports = {
  transpileDependencies: ["vuetify"],
  // devServer: { https: true },
  devServer: {
    proxy: "http://localhost:8011",
    overlay: false,
  },
};
