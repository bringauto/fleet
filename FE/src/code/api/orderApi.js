import { ADD_ORDER, UPDATE_ORDER, DELETE_ORDER } from "../graphql/mutations";
import { apolloClient } from "../../plugins/apollo";
import { GET_ORDERS, GET_ORDER } from "../graphql/queries";

export async function getOrder({ id }) {
  const { data } = await apolloClient.query({
    query: GET_ORDER,
    variables: { orderId: Number(id) },
  });
  return data && data.OrderQuery.orders.nodes[0];
}

export async function getOrders() {
  const { data } = await apolloClient.query({ query: GET_ORDERS });
  return data && data.OrderQuery.orders.nodes;
}

export async function addOrder({ carId, priority, toStationId, arrive }) {
  await apolloClient.mutate({
    mutation: ADD_ORDER,
    variables: {
      priority,
      toStationId,
      carId,
      arrive,
    },
  });
}

export async function updateOrder({ car, from, id, status, priority, to, arrive }) {
  await apolloClient.mutate({
    mutation: UPDATE_ORDER,
    variables: {
      car,
      from,
      id,
      status,
      priority,
      to,
      arrive,
    },
  });
}

export async function deleteOrder(id) {
  await apolloClient.mutate({
    mutation: DELETE_ORDER,
    variables: {
      id,
    },
  });
}
