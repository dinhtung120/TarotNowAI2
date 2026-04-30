# TarotNow Benchmark Analysis

- Run time (UTC): 2026-04-30T07:28:52.002Z
- Base: https://www.tarotnow.xyz/vi
- Benchmark mode: targeted-hotspots
- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader

## Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 0 | 0.0 | 0 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 10 | 33.0 | 3652 | 8 | 0 | 0 | 3 | 1 | yes |
| logged-in-reader | desktop | 9 | 32.6 | 3467 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-out | mobile | 0 | 0.0 | 0 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 10 | 31.9 | 3211 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-reader | mobile | 9 | 32.3 | 3233 | 0 | 0 | 0 | 0 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 1 | 33.0 | 2687 | 0 |
| logged-in-admin | desktop | collection | 1 | 32.0 | 3819 | 8 |
| logged-in-admin | desktop | community | 1 | 41.0 | 6265 | 0 |
| logged-in-admin | desktop | readers | 3 | 31.0 | 2725 | 0 |
| logged-in-admin | desktop | reading | 3 | 33.3 | 4245 | 0 |
| logged-in-admin | desktop | wallet | 1 | 31.0 | 2841 | 0 |
| logged-in-admin | mobile | admin | 1 | 33.0 | 2780 | 0 |
| logged-in-admin | mobile | collection | 1 | 32.0 | 6319 | 0 |
| logged-in-admin | mobile | community | 1 | 32.0 | 3490 | 0 |
| logged-in-admin | mobile | readers | 3 | 31.0 | 2730 | 0 |
| logged-in-admin | mobile | reading | 3 | 32.7 | 2816 | 0 |
| logged-in-admin | mobile | wallet | 1 | 31.0 | 2882 | 0 |
| logged-in-reader | desktop | collection | 1 | 32.0 | 6927 | 0 |
| logged-in-reader | desktop | community | 1 | 32.0 | 3518 | 0 |
| logged-in-reader | desktop | readers | 3 | 31.0 | 2816 | 0 |
| logged-in-reader | desktop | reading | 3 | 35.0 | 3146 | 0 |
| logged-in-reader | desktop | wallet | 1 | 31.0 | 2874 | 0 |
| logged-in-reader | mobile | collection | 1 | 32.0 | 5601 | 0 |
| logged-in-reader | mobile | community | 1 | 32.0 | 3442 | 0 |
| logged-in-reader | mobile | readers | 3 | 31.0 | 2806 | 0 |
| logged-in-reader | mobile | reading | 3 | 34.3 | 2921 | 0 |
| logged-in-reader | mobile | wallet | 1 | 31.0 | 2871 | 0 |

## Key Findings
1. Critical: 7 page(s) vượt 35 requests.
2. High: 38 page(s) vượt 25 requests.
3. High: 4 request vượt 800ms.
4. Medium: 70 request trong dải 400-800ms.
5. Không phát hiện handshake redirect bất thường.
6. Collection-focus: 1 image request(s) >800ms trên 64 image request(s).

## Top Slow Pages
| Scenario | Viewport | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/reading/session/a193d0c6-9a62-42a1-8135-7631ad6d078f | 7007 | 38 | 4932 | 0.0 | 0.0029 |
| logged-in-reader | desktop | /vi/collection | 6927 | 32 | 624 | 13.0 | 0.0026 |
| logged-in-admin | mobile | /vi/collection | 6319 | 32 | 492 | 12.0 | 0.0000 |
| logged-in-admin | desktop | /vi/community | 6265 | 41 | 4076 | 0.0 | 0.0029 |
| logged-in-reader | mobile | /vi/collection | 5601 | 32 | 512 | 9.0 | 0.0000 |
| logged-in-admin | desktop | /vi/collection | 3819 | 32 | 748 | 0.0 | 0.0031 |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | 3630 | 37 | 3596 | 0.0 | 0.0000 |
| logged-in-reader | desktop | /vi/community | 3518 | 32 | 1792 | 0.0 | 0.0026 |
| logged-in-admin | mobile | /vi/community | 3490 | 32 | 1716 | 0.0 | 0.0051 |
| logged-in-reader | mobile | /vi/community | 3442 | 32 | 1644 | 0.0 | 0.0051 |

## High Slow Requests (> 800ms)
| Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-admin | desktop | /vi/reading/session/a193d0c6-9a62-42a1-8135-7631ad6d078f | GET | 200 | 4770 | 3742 | https://www.tarotnow.xyz/vi/reading/session/a193d0c6-9a62-42a1-8135-7631ad6d078f |
| logged-in-admin | desktop | /vi/community | GET | 200 | 3317 | 2905 | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 1059 | 324 | https://www.tarotnow.xyz/vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 |
| logged-in-admin | desktop | /vi/reading/session/9e4d5054-f195-4546-b72a-e6d3b37b7939 | GET | 200 | 857 | 378 | https://www.tarotnow.xyz/vi/reading/session/9e4d5054-f195-4546-b72a-e6d3b37b7939 |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-admin | mobile | /vi/wallet/deposit/history | GET | 200 | 784 | 398 | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | desktop | /vi/wallet/deposit/history | GET | 200 | 755 | 326 | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 752 | 442 | https://www.tarotnow.xyz/vi/collection |
| logged-in-reader | mobile | /vi/reading/session/b48c44dc-9497-49af-8129-a32dd71f65ff | GET | 200 | 751 | 436 | https://www.tarotnow.xyz/vi/reading/session/b48c44dc-9497-49af-8129-a32dd71f65ff |
| logged-in-reader | mobile | /vi/reading/session/b3918917-e26b-429d-b4ee-51dedf38e2cb | GET | 200 | 746 | 343 | https://www.tarotnow.xyz/vi/reading/session/b3918917-e26b-429d-b4ee-51dedf38e2cb |
| logged-in-reader | desktop | /vi/wallet/deposit/history | GET | 200 | 745 | 339 | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-reader | desktop | /vi/reading/session/59c2690a-55fc-42b5-9b56-b672ee0f49e5 | GET | 200 | 745 | 326 | https://www.tarotnow.xyz/vi/reading/session/59c2690a-55fc-42b5-9b56-b672ee0f49e5 |
| logged-in-admin | mobile | /vi/reading/session/b4d47003-af58-4b12-b2ae-381a7db36117 | GET | 200 | 734 | 314 | https://www.tarotnow.xyz/vi/reading/session/b4d47003-af58-4b12-b2ae-381a7db36117 |
| logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 733 | 324 | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 728 | 342 | https://www.tarotnow.xyz/vi/collection |
| logged-in-reader | mobile | /vi/reading/session/a9cf7586-252a-432a-bfc1-c0ecee4aac30 | GET | 200 | 716 | 338 | https://www.tarotnow.xyz/vi/reading/session/a9cf7586-252a-432a-bfc1-c0ecee4aac30 |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 711 | 326 | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | mobile | /vi/wallet/deposit/history | GET | 200 | 703 | 411 | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-reader | desktop | /vi/community | GET | 200 | 696 | 345 | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | mobile | /vi/collection | GET | 200 | 693 | 384 | https://www.tarotnow.xyz/vi/collection |

## Duplicate Request Candidates (Non-telemetry)
| Scenario | Viewport | Route | Count | Request Key |
| --- | --- | --- | ---: | --- |
| - | - | - | - | - |

## Collection Image Metrics
| Scenario | Viewport | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | 17 | 3 | 1 | 17 | 0 | 0 |
| logged-in-reader | desktop | 12 | 3 | 0 | 12 | 0 | 0 |
| logged-in-admin | mobile | 23 | 13 | 0 | 22 | 1 | 0 |
| logged-in-reader | mobile | 12 | 3 | 0 | 12 | 0 | 0 |

## Notes
- Duplicate `/cdn-cgi/rum` được xem là telemetry của Cloudflare, không coi là business over-fetch.
- Coverage dynamic route có thể bị `coverage-blocked` nếu environment không có dữ liệu phù hợp tại thời điểm chạy.