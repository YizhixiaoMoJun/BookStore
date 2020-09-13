import http from 'k6/http';
var sns = [
    "c2e85634-3988-4c00-9523-16297336c208",
    "9b8b1c26-f051-4949-a1da-2a80110df39c",
    "b586b6d9-9be6-4e9d-a614-87a6fbb3141e",
    "44b48f3a-b3f9-42c1-a588-9603735e863b",
    "f82cd999-10a1-4105-814c-1f44a7fc0c7a",
    "9820980b-ccd0-4952-9db4-896cf320c244",
    "ac31dd4b-eaba-4b59-b779-f7055253f90d",
    "7f0bdfcb-8cf9-4990-8ac4-221abbb49169",
    "4dd1eb86-33fc-40c5-ab7b-3a803674b685",
    "149159cb-e5e4-41f2-8c9b-3243daada9a3"
];

export default function() {
    var payload = JSON.stringify({
        orderDetails: [
            { sn: sns[Math.round(Math.random() * sns.length - 1)], count: Math.round(Math.random() * 4) + 1}
        ]
    });

    http.post('http://localhost:5000/book/order', payload, {
        headers: {
            'Content-Type': 'application/json'
        }
    });
};


//k6 run .\book-order.ts --vus 10 --duration 10s