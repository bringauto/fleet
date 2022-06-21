import gql from "graphql-tag";

export const GET_ME = gql`
  query {
    UserQuery {
      me {
        email
        roles
        userName
        tenants {
          nodes {
            name
            id
          }
        }
      }
    }
  }
`;
// Cars
export const GET_CARS = gql`
  query {
    CarQuery {
      cars {
        nodes {
          name
          id
          latitude
          longitude
          status
          fuel
          hwId
          companyName
          carAdminPhone
          callTwiml
          underTest
          speed
          locationHistory(last: 10) {
            nodes {
              id
              dateTime
              latitude
              longitude
            }
          }
        }
      }
    }
  }
`;

export const GET_ONLY_CARS = gql`
  query {
    CarQuery {
      cars {
        nodes {
          name
          id
          latitude
          longitude
          status
          fuel
          hwId
          companyName
          carAdminPhone
          callTwiml
          underTest
          speed
          routeId
        }
      }
    }
  }
`;

export const GET_CARS_ORDERS = gql`
  query {
    CarQuery {
      cars {
        nodes {
          name
          id
          latitude
          longitude
          status
          fuel
          hwId
          companyName
          carAdminPhone
          callTwiml
          underTest
          speed
          locationHistory(first: 10, order_by: { dateTime: DESC }) {
            nodes {
              id
              dateTime
              latitude
              longitude
            }
          }
          orders {
            nodes {
              id
              from {
                id
                name
              }
              to {
                id
                name
              }
              status
              user {
                id
                userName
              }
              priority
              arrive
            }
          }
        }
      }
    }
  }
`;

// Stations
export const GET_STATIONS = gql`
  query {
    StationQuery {
      stations {
        nodes {
          name
          id
          latitude
          longitude
        }
      }
    }
  }
`;

// Orders
export const GET_ORDER = gql`
  query GetOrder($orderId: Int!) {
    OrderQuery {
      orders(where: { id: $orderId }) {
        nodes {
          id
          priority
          car {
            id
            name
          }
          from {
            id
            name
          }
          to {
            id
            name
          }
          status
          arrive
        }
      }
    }
  }
`;

export const GET_ORDERS = gql`
  query GetOrders {
    OrderQuery {
      orders {
        nodes {
          id
          priority
          car {
            id
            name
          }
          from {
            id
            name
          }
          to {
            id
            name
          }
          status
          arrive
        }
      }
    }
  }
`;

// User
export const LOGIN_USER = gql`
  query Login($password: String!, $userName: String!) {
    UserQuery {
      login(login: { password: $password, userName: $userName }) {
        email
        roles
        userName
        tenants {
          nodes {
            name
            id
          }
        }
      }
    }
  }
`;

export const LOGOUT_USER = gql`
  query Logout {
    UserQuery {
      logout
    }
  }
`;

// Routes
export const GET_ROUTES = gql`
  query GetRoutes {
    RouteQuery {
      routes {
        nodes {
          color
          name
          id
          stops {
            latitude
            longitude
            order
            station {
              name
              id
            }
          }
          id
        }
      }
    }
  }
`;
