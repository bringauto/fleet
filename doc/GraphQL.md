
# GraphQL API

- Entry Point HTTPS: https://<our_ip_domain>/graphql
- Entry Point HTTP: http://<our_ip_domain>/graphql

Industrial Portal has a GraphQL API mainly used to access entities like

- [Car]
- [Route]
- [Station]
- [Order]
- [User]

For detailed information and constrains download actual schema from GraphQL entry point.

## Usage

- LogIn by Login Query of the GraphQL API (use [User] credentials),
- save Cookies obtained from Successfully Login Query,
- for each Query/Mutation you want to run you must include these cookie data to authorize yourself.


[Car]: ./Car.md
[Route]: ./Route.md
[Station]: ./Station.md
[Order]: ./Order.md
[User]: ./User.md