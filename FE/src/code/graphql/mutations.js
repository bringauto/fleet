import gql from "graphql-tag";

// Stations
export const CREATE_STATION = gql`
  mutation CreateStation($name: String!, $latitude: Float!, $longitude: Float!) {
    StationMutation {
      addStation(station: { name: $name, latitude: $latitude, longitude: $longitude }) {
        id
      }
    }
  }
`;

export const UPDATE_STATION = gql`
  mutation UpdateStation($id: Int!, $name: String!, $latitude: Float!, $longitude: Float!) {
    StationMutation {
      updateStation(station: { id: $id, name: $name, latitude: $latitude, longitude: $longitude }) {
        id
      }
    }
  }
`;

export const DELETE_STATION = gql`
  mutation DeleteStation($id: Int!) {
    StationMutation {
      deleteStation(stationId: $id) {
        id
      }
    }
  }
`;

// Orders
export const ADD_ORDER = gql`
  mutation AddOrder($toStationId: Int!, $priority: String!, $carId: Int!, $arrive: DateTime) {
    OrderMutation {
      addOrder(
        order: { carId: $carId, toStationId: $toStationId, priority: $priority, arrive: $arrive }
      ) {
        car {
          name
        }
        priority
      }
    }
  }
`;

export const UPDATE_ORDER = gql`
  mutation UpdateOrder(
    $car: Int!
    $from: Int
    $id: Int!
    $status: String!
    $priority: String!
    $to: Int!
    $arrive: DateTime
  ) {
    OrderMutation {
      updateOrder(
        order: {
          carId: $car
          fromStationId: $from
          id: $id
          status: $status
          priority: $priority
          toStationId: $to
          arrive: $arrive
        }
      ) {
        id
      }
    }
  }
`;

export const DELETE_ORDER = gql`
  mutation DeleteOrder($id: Int!) {
    OrderMutation {
      deleteOrder(orderId: $id) {
        id
      }
    }
  }
`;

// Cars
export const UPDATE_CAR = gql`
  mutation UpdateCar(
    $id: Int!
    $name: String!
    $status: String!
    $companyName: String
    $hwId: String
    $carAdminPhone: String
    $routeId: Int
    $underTest: Boolean!
  ) {
    CarMutation {
      updateCar(
        car: {
          id: $id
          name: $name
          requireNewToken: false
          status: $status
          companyName: $companyName
          button: NORMAL
          hwId: $hwId
          underTest: $underTest
          routeId: $routeId
          carAdminPhone: $carAdminPhone
        }
      ) {
        id
      }
    }
  }
`;

export const ADD_CAR = gql`
  mutation AddCar(
    $carAdminPhone: String
    $companyName: String
    $hwId: String
    $name: String
    $routeId: Int
    $underTest: Boolean!
  ) {
    CarMutation {
      addCar(
        car: {
          carAdminPhone: $carAdminPhone
          companyName: $companyName
          hwId: $hwId
          name: $name
          routeId: $routeId
          underTest: $underTest
        }
      ) {
        id
      }
    }
  }
`;

export const DELETE_CAR = gql`
  mutation DeleteCar($id: Int!) {
    CarMutation {
      deleteCar(carId: $id) {
        id
      }
    }
  }
`;

// Routes
export const ADD_ROUTE = gql`
  mutation AddRoute($color: String, $name: String, $stops: [RouteStopInput]) {
    RouteMutation {
      addRoute(route: { color: $color, name: $name, stops: $stops }) {
        id
      }
    }
  }
`;

export const UPDATE_ROUTE = gql`
  mutation UpdateRoute($id: Int!, $color: String, $name: String, $stops: [RouteStopUpdateInput]) {
    RouteMutation {
      updateRoute(route: { id: $id, color: $color, name: $name, stops: $stops }) {
        id
      }
    }
  }
`;

export const DELETE_ROUTE = gql`
  mutation DeleteRoute($id: Int!) {
    RouteMutation {
      deleteRoute(routeId: $id) {
        id
      }
    }
  }
`;
