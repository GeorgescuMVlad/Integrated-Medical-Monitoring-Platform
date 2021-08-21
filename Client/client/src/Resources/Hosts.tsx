export let url =
    process.env.NODE_ENV === "development"
        ? "http://localhost:52338"
        : "https://a1-backend-georgescu-vlad.herokuapp.com";
