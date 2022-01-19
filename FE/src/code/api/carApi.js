import { GET_CARS, GET_CARS_ORDERS, GET_ONLY_CARS } from "../graphql/queries";
import { UPDATE_CAR, DELETE_CAR, ADD_CAR } from "../graphql/mutations";
import { apolloClient } from "../../plugins/apollo";

export async function getCars() {
  const { data } = await apolloClient.query({
    query: GET_CARS,
  });
  return data && data.CarQuery.cars.nodes;
}
export async function getCarsWithoutHistory() {
  const { data } = await apolloClient.query({
    query: GET_ONLY_CARS,
  });
  return data && data.CarQuery.cars.nodes;
}

export async function getCarsWithOrders() {
  const { data } = await apolloClient.query({
    query: GET_CARS_ORDERS,
  });
  return data && data.CarQuery.cars.nodes;
}

export async function updateCar({
  id,
  name,
  status,
  companyName,
  hwId,
  carAdminPhone,
  routeId,
  underTest,
}) {
  await apolloClient.mutate({
    mutation: UPDATE_CAR,
    variables: {
      id,
      name,
      status,
      companyName,
      hwId,
      carAdminPhone,
      routeId,
      underTest,
    },
  });
}

export async function deleteCar(id) {
  await apolloClient.mutate({
    mutation: DELETE_CAR,
    variables: {
      id,
    },
  });
}

export async function addCar({ carAdminPhone, companyName, hwId, name, routeId, underTest }) {
  await apolloClient.mutate({
    mutation: ADD_CAR,
    variables: {
      carAdminPhone,
      companyName,
      hwId,
      name,
      routeId,
      underTest,
    },
  });
}
