import Vue from "vue";
import { ApolloClient } from "apollo-client";
import { createHttpLink } from "apollo-link-http";
import { InMemoryCache } from "apollo-cache-inmemory";
import VueApollo from "vue-apollo";
import { setContext } from "apollo-link-context";
import VueStore from "../store";

// HTTP connection to the API
export const httpLink = createHttpLink({
  uri: "/graphql",
  credentials: "include",
});

const authLink = setContext(async (_, { headers }) => {
  return {
    headers: {
      ...headers,
      tenant: VueStore.state.user ? VueStore.state.user.tenants.nodes[0].id : "",
    },
  };
});

// Cache implementation
const cache = new InMemoryCache();

// Create the apollo client
export const apolloClient = new ApolloClient({
  link: authLink.concat(httpLink),
  cache,
  defaultOptions: {
    query: {
      fetchPolicy: "no-cache",
    },
    watchQuery: {
      fetchPolicy: "no-cache",
    },
  },
});

export const apolloProvider = new VueApollo({
  defaultClient: apolloClient,
});

Vue.use(VueApollo);
