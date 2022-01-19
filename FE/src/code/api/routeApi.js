import { GET_ROUTES } from "../graphql/queries";
import { ADD_ROUTE, UPDATE_ROUTE, DELETE_ROUTE } from "../graphql/mutations";

import { apolloClient } from "../../plugins/apollo";

export function getMappedRoutes(routes) {
  return routes.map((r) => {
    r.stops.sort((a, b) => a.order - b.order);
    return {
      ...r,
      stops: r.stops.map((s) => {
        return [s.latitude, s.longitude];
      }),
    };
  });
}

export async function getRoutes(mapped) {
  const { data } = await apolloClient.query({ query: GET_ROUTES });
  const routes = data && data.RouteQuery.routes.nodes;
  if (mapped) {
    return getMappedRoutes(routes);
  }
  return routes.map((r) => {
    return { ...r, stops: r.stops.sort((a, b) => a.order - b.order) };
  });
}

export async function addRoute({ color, name, stops }) {
  await apolloClient.mutate({
    mutation: ADD_ROUTE,
    variables: {
      color,
      name,
      stops,
    },
  });
}

export async function updateRoute({ id, color, name, stops }) {
  const mappedStops = stops.map((stop) => {
    const dto = {
      longitude: stop.longitude,
      latitude: stop.latitude,
      order: stop.order,
    };
    if (stop.station && stop.station.id) {
      dto.stationId = stop.station.id;
    }
    return dto;
  });
  await apolloClient.mutate({
    mutation: UPDATE_ROUTE,
    variables: {
      id,
      color,
      name,
      stops: mappedStops,
    },
  });
}

export async function deleteRoute(id) {
  await apolloClient.mutate({
    mutation: DELETE_ROUTE,
    variables: {
      id,
    },
  });
}
