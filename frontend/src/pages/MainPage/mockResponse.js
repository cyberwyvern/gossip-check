let response = `
{
    "verdict": "MostLikelyTrue",
    "relatedArticles": [
        {
            "articleUrl": "https://news.yahoo.com/peru-election-fujimori-edges-ahead-080029168.html",
            "stance": "Agree",
            "factuality": "High",
            "bias": "Left center",
            "mediaType": "Website",
            "popularity": "High traffic",
            "mbfcCredibilityRating": "High credibility",
            "reasoning": null,
            "country": "USA",
            "worldPressFreedomRank": 45,
            "mbfcPageUrl": "https://mediabiasfactcheck.com/yahoo-news/"
        },
        {
            "articleUrl": "https://www.irishtimes.com/news/world/peru-election-on-knife-edge-as-leftist-castillo-closes-fujimori-s-lead-1.4586805",
            "stance": "Agree",
            "factuality": "High",
            "bias": null,
            "mediaType": null,
            "popularity": null,
            "mbfcCredibilityRating": null,
            "reasoning": null,
            "country": "Ireland",
            "worldPressFreedomRank": null,
            "mbfcPageUrl": "https://mediabiasfactcheck.com/the-irish-times/"
        },
        {
            "articleUrl": "https://www.reuters.com/world/americas/peru-awakes-uncertain-future-with-polarized-vote-knife-edge-2021-06-07/",
            "stance": "Agree",
            "factuality": "Very high",
            "bias": "Least biased",
            "mediaType": "News agency",
            "popularity": "High traffic",
            "mbfcCredibilityRating": "High credibility",
            "reasoning": null,
            "country": "United Kingdom",
            "worldPressFreedomRank": 35,
            "mbfcPageUrl": "https://mediabiasfactcheck.com/reuters/"
        },
        {
            "articleUrl": "https://japantoday.com/category/world/update-7-peru-socialist-castillo-extends-narrow-lead-in-polarized-vote",
            "stance": "Agree",
            "factuality": "High",
            "bias": null,
            "mediaType": null,
            "popularity": null,
            "mbfcCredibilityRating": null,
            "reasoning": null,
            "country": "Japan",
            "worldPressFreedomRank": null,
            "mbfcPageUrl": "https://mediabiasfactcheck.com/japan-today/"
        }
    ]
}`

response = JSON.parse(response);
export { response }