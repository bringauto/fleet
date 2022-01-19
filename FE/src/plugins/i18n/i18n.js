import Vue from "vue";
import VueI18n from "vue-i18n";
import translations from "./translations";
import dateTimeFormats from "./dateTimeFormats";

Vue.use(VueI18n);

// eslint-disable-next-line import/prefer-default-export
const i18n = new VueI18n({
  locales: ["en", "cs"],
  locale: "cs",
  messages: translations,
  dateTimeFormats,
});

function getBrowserLang() {
  const languages = Object.keys(i18n.messages);
  const browserLang = ((navigator.languages && navigator.languages[0]) || "").substr(0, 2);
  return languages.includes(browserLang) ? browserLang : false;
}

function getSavedLang() {
  return localStorage.getItem("language");
}

function setLang() {
  const saved = getSavedLang();
  const browser = getBrowserLang();
  if (saved) {
    console.log("set from localstorage");
    return saved;
  }
  if (browser) {
    console.log("set from browser");
    return browser;
  }

  console.log("set from default");
  return "cs";
}

i18n.locale = setLang();

export default i18n;
