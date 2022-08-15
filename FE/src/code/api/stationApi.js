import { GET_STATIONS } from "../graphql/queries";
import { CREATE_STATION, DELETE_STATION, UPDATE_STATION } from "../graphql/mutations";
import { apolloClient } from "../../plugins/apollo";

export async function getStations() {
  const { data } = await apolloClient.query({ query: GET_STATIONS });
  return data && data.StopQuery.stop.nodes;
}

export async function createStation({ name, latitude, longitude }) {
  await apolloClient.mutate({
    mutation: CREATE_STATION,
    variables: {
      name,
      latitude,
      longitude,
    },
  });
}

export async function updateStation({ id, name, latitude, longitude }) {
  await apolloClient.mutate({
    mutation: UPDATE_STATION,
    variables: {
      id,
      name,
      latitude,
      longitude,
    },
  });
}

export async function deleteStation(id) {
  await apolloClient.mutate({
    mutation: DELETE_STATION,
    variables: {
      id,
    },
  });
}
