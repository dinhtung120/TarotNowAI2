# TarotNow Navigation Benchmark Report

- Generated at (UTC): 2026-05-08T16:07:29.424Z
- Base URL: https://www.tarotnow.xyz/vi
- Benchmark mode: targeted-hotspots
- Thresholds: >35 requests = Critical, >25 = High, request >800ms = High, >400ms = Medium
- Critical pages (request count): 5
- High pages (request count): 38
- High slow requests: 45
- Medium slow requests: 66

## Scenario Summary
| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Document Reloads | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 0 | 0 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 10 | 3823 | 321 | 1 | 0 | 8 | 0 | yes |
| logged-in-reader | desktop | 9 | 5437 | 288 | 15 | 0 | 14 | 0 | yes |
| logged-out | mobile | 0 | 0 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 10 | 3519 | 303 | 0 | 0 | 3 | 0 | yes |
| logged-in-reader | mobile | 9 | 3713 | 267 | 0 | 0 | 1 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 1 | 29.0 | 3204 | 0 |
| logged-in-admin | desktop | collection | 1 | 33.0 | 6835 | 1 |
| logged-in-admin | desktop | community | 1 | 34.0 | 4656 | 0 |
| logged-in-admin | desktop | readers | 3 | 29.7 | 3484 | 0 |
| logged-in-admin | desktop | reading | 3 | 35.3 | 3368 | 0 |
| logged-in-admin | desktop | wallet | 1 | 30.0 | 2974 | 0 |
| logged-in-admin | mobile | admin | 1 | 29.0 | 2806 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 5526 | 0 |
| logged-in-admin | mobile | community | 1 | 34.0 | 4075 | 0 |
| logged-in-admin | mobile | readers | 3 | 29.7 | 3271 | 0 |
| logged-in-admin | mobile | reading | 3 | 29.3 | 3087 | 0 |
| logged-in-admin | mobile | wallet | 1 | 34.0 | 3705 | 0 |
| logged-in-reader | desktop | collection | 1 | 32.0 | 7992 | 4 |
| logged-in-reader | desktop | community | 1 | 37.0 | 5712 | 0 |
| logged-in-reader | desktop | readers | 3 | 29.7 | 5351 | 6 |
| logged-in-reader | desktop | reading | 3 | 33.7 | 4909 | 5 |
| logged-in-reader | desktop | wallet | 1 | 29.0 | 4447 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 5518 | 0 |
| logged-in-reader | mobile | community | 1 | 34.0 | 5880 | 0 |
| logged-in-reader | mobile | readers | 3 | 28.7 | 2827 | 0 |
| logged-in-reader | mobile | reading | 3 | 30.0 | 3486 | 0 |
| logged-in-reader | mobile | wallet | 1 | 28.0 | 3079 | 0 |

## Per-Page Metrics
| Scenario | Viewport | Feature | Route | Requests | Interaction Requests | Severity | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Collection Img Requests | Collection Img 400-800ms | Collection Img >800ms | Collection Img First Load | Collection Img Reopen | Collection Img Cache Hits | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |
| --- | --- | --- | --- | ---: | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3204 | 750 | 1187 | 616 | 992 | 0.0000 | 0.0 | 647487 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 30 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2974 | 765 | 965 | 632 | 1080 | 0.0042 | 0.0 | 634711 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 34 | 7 | high | 0 | 0 | 0 | 0 | 2 | 2 | 0 | 2 | 0 | 0 | 4656 | 807 | 1878 | 612 | 1788 | 0.0042 | 0.0 | 776624 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 33 | 38 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6835 | 757 | 1261 | 680 | 1052 | 0.0043 | 0.0 | 647276 |
| logged-in-admin | desktop | reading | /vi/reading/session/c129bc4c-42f0-48f5-b215-372df9b190ed | 28 | 12 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2796 | 773 | 794 | 644 | 1048 | 0.0042 | 0.0 | 632394 |
| logged-in-admin | desktop | reading | /vi/reading/session/6edd7197-a24d-4d21-9f84-53dcfc98ff09 | 39 | 1 | critical | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3730 | 732 | 1718 | 668 | 3348 | 0.0042 | 0.0 | 738241 |
| logged-in-admin | desktop | reading | /vi/reading/session/41058d9b-941d-4d9b-a905-a9f5073451ec | 39 | 1 | critical | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3578 | 808 | 1576 | 700 | 3388 | 0.0042 | 0.0 | 738365 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 33 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4258 | 772 | 2255 | 656 | 1020 | 0.0042 | 0.0 | 643750 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3399 | 827 | 1396 | 640 | 996 | 0.0042 | 0.0 | 631263 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2795 | 778 | 791 | 608 | 948 | 0.0042 | 0.0 | 631222 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 29 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4447 | 2436 | 2436 | 876 | 2528 | 0.0039 | 0.0 | 632774 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 37 | 4 | critical | 0 | 0 | 2 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 5712 | 2850 | 2964 | 844 | 4040 | 0.0039 | 0.0 | 779793 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 32 | 2 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 7992 | 4955 | 4955 | 2420 | 5000 | 0.0039 | 24.0 | 641997 |
| logged-in-reader | desktop | reading | /vi/reading/session/9f154052-ee11-49da-a0c2-7e7e9619e9b7 | 36 | 4 | critical | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5776 | 3627 | 3768 | 1568 | 3676 | 0.0039 | 0.0 | 727779 |
| logged-in-reader | desktop | reading | /vi/reading/session/6824a31f-8492-49af-bf22-4d1a164b24f8 | 36 | 4 | critical | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4799 | 2661 | 2791 | 792 | 2736 | 0.0039 | 0.0 | 727763 |
| logged-in-reader | desktop | reading | /vi/reading/session/a5f8f1c9-6d19-4afc-a247-c624365ff6bc | 29 | 8 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4153 | 2145 | 2145 | 792 | 2424 | 0.0039 | 0.0 | 632407 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 31 | 1 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6378 | 4368 | 4369 | 2644 | 4396 | 0.0039 | 0.0 | 631287 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 29 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5794 | 3774 | 3775 | 1988 | 3836 | 0.0039 | 0.0 | 632313 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3882 | 1872 | 1872 | 836 | 1924 | 0.0039 | 0.0 | 631199 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2806 | 787 | 800 | 592 | 912 | 0.0000 | 0.0 | 647868 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 34 | 1 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3705 | 713 | 1700 | 604 | 944 | 0.0000 | 0.0 | 645879 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 34 | 6 | high | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 4075 | 790 | 1309 | 680 | 1816 | 0.0122 | 0.0 | 776894 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | 37 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5526 | 770 | 793 | 564 | 564 | 0.0000 | 0.0 | 642963 |
| logged-in-admin | mobile | reading | /vi/reading/session/a2609bd4-44b1-4b40-a8bd-81b1980aac09 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2813 | 776 | 808 | 576 | 904 | 0.0000 | 0.0 | 632181 |
| logged-in-admin | mobile | reading | /vi/reading/session/0d01e4f9-623b-47ee-809c-5ec9d8edb6be | 30 | 8 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3040 | 790 | 1034 | 552 | 884 | 0.0071 | 0.0 | 681066 |
| logged-in-admin | mobile | reading | /vi/reading/session/18b9cb87-1bbd-4bb9-8b20-bdd137ccb0d9 | 30 | 8 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3407 | 1255 | 1400 | 708 | 1256 | 0.0072 | 0.0 | 681016 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3025 | 778 | 1020 | 736 | 1056 | 0.0000 | 0.0 | 631499 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3383 | 737 | 1375 | 740 | 740 | 0.0000 | 0.0 | 643900 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3405 | 713 | 1398 | 576 | 896 | 0.0071 | 0.0 | 631177 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3079 | 711 | 1071 | 580 | 924 | 0.0071 | 0.0 | 631795 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 34 | 7 | high | 0 | 0 | 0 | 0 | 2 | 2 | 0 | 2 | 0 | 0 | 5880 | 725 | 3103 | 544 | 1668 | 0.0173 | 0.0 | 776542 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | 30 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5518 | 747 | 787 | 556 | 556 | 0.0000 | 0.0 | 642184 |
| logged-in-reader | mobile | reading | /vi/reading/session/9f9f51dd-a2a1-4a56-a999-a748edc14160 | 30 | 8 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3781 | 696 | 1776 | 560 | 884 | 0.0071 | 0.0 | 681190 |
| logged-in-reader | mobile | reading | /vi/reading/session/b3592556-bd0f-4779-8274-a037973bd26e | 30 | 8 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3477 | 707 | 1470 | 548 | 876 | 0.0071 | 0.0 | 681257 |
| logged-in-reader | mobile | reading | /vi/reading/session/d512f861-d1df-498b-99d1-d9eb0566f847 | 30 | 8 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3199 | 742 | 1187 | 592 | 920 | 0.0071 | 0.0 | 680994 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 30 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2942 | 869 | 932 | 588 | 908 | 0.0000 | 0.0 | 633543 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2739 | 704 | 731 | 556 | 892 | 0.0000 | 0.0 | 631389 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2800 | 767 | 792 | 544 | 864 | 0.0000 | 0.0 | 631472 |

## Collection Image Focus
| Scenario | Viewport | Route | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/collection | 0 | 0 | 0 | 0 | 0 | 0 |
| logged-in-reader | desktop | /vi/collection | 0 | 0 | 0 | 0 | 0 | 0 |
| logged-in-admin | mobile | /vi/collection | 0 | 0 | 0 | 0 | 0 | 0 |
| logged-in-reader | mobile | /vi/collection | 0 | 0 | 0 | 0 | 0 | 0 |

## Suspicious Pages (>25 requests)
| Scenario | Viewport | Feature | Route | Request Count | Severity | API | Static | Third-party |
| --- | --- | --- | --- | ---: | --- | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 30 | high | 2 | 26 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 34 | high | 0 | 32 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 33 | high | 4 | 27 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/c129bc4c-42f0-48f5-b215-372df9b190ed | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/6edd7197-a24d-4d21-9f84-53dcfc98ff09 | 39 | critical | 6 | 31 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/41058d9b-941d-4d9b-a905-a9f5073451ec | 39 | critical | 6 | 31 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 33 | high | 4 | 27 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 37 | critical | 3 | 32 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 32 | high | 3 | 27 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/9f154052-ee11-49da-a0c2-7e7e9619e9b7 | 36 | critical | 4 | 30 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/6824a31f-8492-49af-bf22-4d1a164b24f8 | 36 | critical | 4 | 30 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/a5f8f1c9-6d19-4afc-a247-c624365ff6bc | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 31 | high | 3 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 34 | high | 5 | 27 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 34 | high | 0 | 32 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/a2609bd4-44b1-4b40-a8bd-81b1980aac09 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/0d01e4f9-623b-47ee-809c-5ec9d8edb6be | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/18b9cb87-1bbd-4bb9-8b20-bdd137ccb0d9 | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 4 | 27 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 34 | high | 0 | 32 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/9f9f51dd-a2a1-4a56-a999-a748edc14160 | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/b3592556-bd0f-4779-8274-a037973bd26e | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/d512f861-d1df-498b-99d1-d9eb0566f847 | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |

## High Slow Requests (>800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | GET | 200 | 4805 | 2232 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 4301 | 2434 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 3726 | 1780 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-reader | desktop | reading | /vi/reading/session/9f154052-ee11-49da-a0c2-7e7e9619e9b7 | GET | 200 | 3554 | 1374 | html | https://www.tarotnow.xyz/vi/reading/session/9f154052-ee11-49da-a0c2-7e7e9619e9b7 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 2803 | 544 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 2759 | 2742 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | desktop | reading | /vi/reading/session/6824a31f-8492-49af-bf22-4d1a164b24f8 | GET | 200 | 2588 | 490 | html | https://www.tarotnow.xyz/vi/reading/session/6824a31f-8492-49af-bf22-4d1a164b24f8 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 2389 | 594 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-reader | desktop | reading | /vi/reading/session/a5f8f1c9-6d19-4afc-a247-c624365ff6bc | GET | 200 | 2093 | 516 | html | https://www.tarotnow.xyz/vi/reading/session/a5f8f1c9-6d19-4afc-a247-c624365ff6bc |
| logged-in-reader | desktop | reading | /vi/reading/session/9f154052-ee11-49da-a0c2-7e7e9619e9b7 | GET | 200 | 2049 | 517 | api | https://www.tarotnow.xyz/api/reading/cards-catalog |
| logged-in-reader | desktop | reading | /vi/reading/session/6824a31f-8492-49af-bf22-4d1a164b24f8 | GET | 200 | 2028 | 768 | api | https://www.tarotnow.xyz/api/reading/cards-catalog |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 1927 | 780 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 1819 | 525 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-reader | desktop | reading | /vi/reading/session/9f154052-ee11-49da-a0c2-7e7e9619e9b7 | GET | 200 | 1637 | 1225 | api | https://www.tarotnow.xyz/api/auth/session |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 1579 | 1219 | api | https://www.tarotnow.xyz/api/auth/session |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 1513 | 1499 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | reading | /vi/reading/session/9f9f51dd-a2a1-4a56-a999-a748edc14160 | GET | 200 | 1434 | 1417 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1417 | 1109 | api | https://www.tarotnow.xyz/api/auth/session |
| logged-in-admin | desktop | reading | /vi/reading/session/6edd7197-a24d-4d21-9f84-53dcfc98ff09 | GET | 200 | 1397 | 337 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 1361 | 503 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | reading | /vi/reading/session/9f154052-ee11-49da-a0c2-7e7e9619e9b7 | GET | 200 | 1359 | 953 | api | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-reader | desktop | reading | /vi/reading/session/6824a31f-8492-49af-bf22-4d1a164b24f8 | GET | 200 | 1355 | 929 | api | https://www.tarotnow.xyz/api/auth/session |
| logged-in-admin | desktop | reading | /vi/reading/session/41058d9b-941d-4d9b-a905-a9f5073451ec | GET | 200 | 1241 | 409 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | reading | /vi/reading/session/18b9cb87-1bbd-4bb9-8b20-bdd137ccb0d9 | GET | 200 | 1205 | 434 | html | https://www.tarotnow.xyz/vi/reading/session/18b9cb87-1bbd-4bb9-8b20-bdd137ccb0d9 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 1112 | 451 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-reader | mobile | reading | /vi/reading/session/b3592556-bd0f-4779-8274-a037973bd26e | GET | 200 | 1112 | 1101 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | desktop | reading | /vi/reading/session/9f154052-ee11-49da-a0c2-7e7e9619e9b7 | GET | 200 | 1083 | 620 | api | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1049 | 304 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | reading | /vi/reading/session/6824a31f-8492-49af-bf22-4d1a164b24f8 | GET | 200 | 1042 | 765 | api | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 1041 | 1026 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1026 | 1013 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 979 | 480 | api | https://www.tarotnow.xyz/api/auth/session |
| logged-in-admin | mobile | reading | /vi/reading/session/18b9cb87-1bbd-4bb9-8b20-bdd137ccb0d9 | GET | 200 | 935 | 920 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 934 | 923 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 930 | 916 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | desktop | reading | /vi/reading/session/9f154052-ee11-49da-a0c2-7e7e9619e9b7 | GET | 200 | 918 | 907 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 896 | 418 | api | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 885 | 873 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | GET | 200 | 875 | 727 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | reading | /vi/reading/session/d512f861-d1df-498b-99d1-d9eb0566f847 | GET | 200 | 843 | 825 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 832 | 414 | api | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-reader | mobile | reading | /vi/reading/session/d512f861-d1df-498b-99d1-d9eb0566f847 | GET | 200 | 827 | 813 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 816 | 323 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 812 | 564 | static | https://www.tarotnow.xyz/_next/static/chunks/0~.ygy4-u1lnf.js |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 805 | 555 | static | https://www.tarotnow.xyz/_next/static/chunks/0~9zvxu5gce~4.js |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-reader | desktop | reading | /vi/reading/session/6824a31f-8492-49af-bf22-4d1a164b24f8 | GET | 200 | 795 | 788 | api | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 792 | 775 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 790 | 403 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 786 | 305 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 779 | 303 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 771 | 308 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 770 | 344 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 765 | 743 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | reading | /vi/reading/session/18b9cb87-1bbd-4bb9-8b20-bdd137ccb0d9 | GET | 200 | 758 | 375 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | reading | /vi/reading/session/18b9cb87-1bbd-4bb9-8b20-bdd137ccb0d9 | GET | 200 | 752 | 131 | static | https://www.tarotnow.xyz/_next/static/chunks/0b8cs8wbqiq6e.js |
| logged-in-admin | desktop | reading | /vi/reading/session/41058d9b-941d-4d9b-a905-a9f5073451ec | GET | 200 | 750 | 325 | html | https://www.tarotnow.xyz/vi/reading/session/41058d9b-941d-4d9b-a905-a9f5073451ec |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 749 | 334 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 746 | 345 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | GET | 200 | 724 | 313 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-admin | mobile | reading | /vi/reading/session/0d01e4f9-623b-47ee-809c-5ec9d8edb6be | GET | 200 | 724 | 312 | html | https://www.tarotnow.xyz/vi/reading/session/0d01e4f9-623b-47ee-809c-5ec9d8edb6be |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 722 | 321 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-admin | desktop | reading | /vi/reading/session/c129bc4c-42f0-48f5-b215-372df9b190ed | GET | 200 | 721 | 321 | html | https://www.tarotnow.xyz/vi/reading/session/c129bc4c-42f0-48f5-b215-372df9b190ed |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 721 | 325 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 720 | 317 | html | https://www.tarotnow.xyz/vi/admin |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 717 | 320 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 717 | 314 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-admin | mobile | reading | /vi/reading/session/a2609bd4-44b1-4b40-a8bd-81b1980aac09 | GET | 200 | 714 | 319 | html | https://www.tarotnow.xyz/vi/reading/session/a2609bd4-44b1-4b40-a8bd-81b1980aac09 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 712 | 702 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | GET | 200 | 701 | 377 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | GET | 200 | 697 | 317 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-admin | mobile | reading | /vi/reading/session/0d01e4f9-623b-47ee-809c-5ec9d8edb6be | GET | 200 | 693 | 680 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | reading | /vi/reading/session/d512f861-d1df-498b-99d1-d9eb0566f847 | GET | 200 | 692 | 319 | html | https://www.tarotnow.xyz/vi/reading/session/d512f861-d1df-498b-99d1-d9eb0566f847 |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 682 | 667 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 681 | 319 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 677 | 329 | html | https://www.tarotnow.xyz/vi/admin |
| logged-in-admin | mobile | reading | /vi/reading/session/18b9cb87-1bbd-4bb9-8b20-bdd137ccb0d9 | GET | 200 | 675 | 663 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | desktop | reading | /vi/reading/session/6edd7197-a24d-4d21-9f84-53dcfc98ff09 | GET | 200 | 674 | 313 | html | https://www.tarotnow.xyz/vi/reading/session/6edd7197-a24d-4d21-9f84-53dcfc98ff09 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 670 | 314 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 669 | 322 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 667 | 653 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 663 | 330 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 663 | 321 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 661 | 331 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 658 | 324 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-reader | mobile | reading | /vi/reading/session/b3592556-bd0f-4779-8274-a037973bd26e | GET | 200 | 655 | 326 | html | https://www.tarotnow.xyz/vi/reading/session/b3592556-bd0f-4779-8274-a037973bd26e |
| logged-in-reader | mobile | reading | /vi/reading/session/9f9f51dd-a2a1-4a56-a999-a748edc14160 | GET | 200 | 649 | 317 | html | https://www.tarotnow.xyz/vi/reading/session/9f9f51dd-a2a1-4a56-a999-a748edc14160 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 629 | 360 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | reading | /vi/reading/session/6edd7197-a24d-4d21-9f84-53dcfc98ff09 | GET | 200 | 552 | 314 | api | https://www.tarotnow.xyz/api/reading/cards-catalog |
| logged-in-admin | desktop | reading | /vi/reading/session/41058d9b-941d-4d9b-a905-a9f5073451ec | GET | 200 | 547 | 313 | api | https://www.tarotnow.xyz/api/reading/cards-catalog |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | GET | 200 | 523 | 512 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 467 | 432 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 463 | 429 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 461 | 136 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 461 | 111 | static | https://www.tarotnow.xyz/_next/static/chunks/0b8cs8wbqiq6e.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 449 | 277 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 448 | 379 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 441 | 96 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | GET | 200 | 431 | 417 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 426 | 366 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 425 | 149 | static | https://www.tarotnow.xyz/_next/static/chunks/0b8cs8wbqiq6e.js |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 424 | 392 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 424 | 375 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 422 | 144 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 413 | 215 | static | https://www.tarotnow.xyz/_next/static/chunks/0b8cs8wbqiq6e.js |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 410 | 379 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | reading | /vi/reading/session/18b9cb87-1bbd-4bb9-8b20-bdd137ccb0d9 | GET | 200 | 410 | 134 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | desktop | reading | /vi/reading/session/41058d9b-941d-4d9b-a905-a9f5073451ec | GET | 200 | 409 | 364 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 406 | 162 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | GET | 200 | 405 | 387 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 403 | 155 | static | https://www.tarotnow.xyz/_next/static/chunks/0~9zvxu5gce~4.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 401 | 375 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |

## Pending Requests
| Scenario | Viewport | Route | URL |
| --- | --- | --- | --- |
| logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/api/collection/card-image/81a3d9698977fda2.avif?src=https%3A%2F%2Fimg.tarotnow.xyz%2Flight-god-50%2F15_Temperance_50_20260325_181356.avif%3Fiv%3D81a3d9698977fda2%26variant%3Dthumb&iv=81a3d9698977fda2 |
| logged-in-reader | desktop | /vi/collection | https://www.tarotnow.xyz/api/auth/session |
| logged-in-reader | desktop | /vi/collection | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-reader | desktop | /vi/collection | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-reader | desktop | /vi/collection | https://www.tarotnow.xyz/api/v1/reading/cards-catalog/manifest |
| logged-in-reader | desktop | /vi/reading/session/9f154052-ee11-49da-a0c2-7e7e9619e9b7 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | desktop | /vi/reading/session/a5f8f1c9-6d19-4afc-a247-c624365ff6bc | https://www.tarotnow.xyz/api/auth/session |
| logged-in-reader | desktop | /vi/reading/session/a5f8f1c9-6d19-4afc-a247-c624365ff6bc | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-reader | desktop | /vi/reading/session/a5f8f1c9-6d19-4afc-a247-c624365ff6bc | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-reader | desktop | /vi/reading/session/a5f8f1c9-6d19-4afc-a247-c624365ff6bc | https://www.tarotnow.xyz/api/reading/cards-catalog |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | https://www.tarotnow.xyz/api/auth/session |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/api/auth/session |
| logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/api/auth/session?mode=lite |

## Coverage Notes
| Scenario | Viewport | Note |
| --- | --- | --- |
| logged-out | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-out | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-out | desktop | dynamic-routes: skipped for logged-out scenario. |
| logged-out | desktop | scenario-filter:logged-out-protected-routes-skipped=4 |
| logged-in-admin | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-admin | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-in-admin | desktop | reading.init.daily_1: blocked (400). |
| logged-in-admin | desktop | reading.init.spread_3: created c129bc4c-42f0-48f5-b215-372df9b190ed. |
| logged-in-admin | desktop | reading.init.spread_5: created 6edd7197-a24d-4d21-9f84-53dcfc98ff09. |
| logged-in-admin | desktop | reading.init.spread_10: created 41058d9b-941d-4d9b-a905-a9f5073451ec. |
| logged-in-admin | desktop | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-admin | desktop | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-admin | desktop | reader-detail:ui-discovery-empty |
| logged-in-admin | desktop | chat-room-detail:ui-discovery-empty |
| logged-in-admin | desktop | reading-history-detail:ui-discovery-empty |
| logged-in-admin | desktop | community-posts:api-discovery-1 |
| logged-in-admin | desktop | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-reader | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-reader | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-in-reader | desktop | reading.init.daily_1: blocked (400). |
| logged-in-reader | desktop | reading.init.spread_3: created 9f154052-ee11-49da-a0c2-7e7e9619e9b7. |
| logged-in-reader | desktop | reading.init.spread_5: created 6824a31f-8492-49af-bf22-4d1a164b24f8. |
| logged-in-reader | desktop | reading.init.spread_10: created a5f8f1c9-6d19-4afc-a247-c624365ff6bc. |
| logged-in-reader | desktop | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-reader | desktop | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-reader | desktop | reader-detail:ui-discovery-empty |
| logged-in-reader | desktop | chat-room-detail:ui-discovery-empty |
| logged-in-reader | desktop | reading-history-detail:ui-discovery-empty |
| logged-in-reader | desktop | community-posts:api-discovery-1 |
| logged-in-reader | desktop | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-reader | desktop | scenario-filter:reader-admin-routes-skipped=1 |
| logged-out | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-out | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-out | mobile | dynamic-routes: skipped for logged-out scenario. |
| logged-out | mobile | scenario-filter:logged-out-protected-routes-skipped=4 |
| logged-in-admin | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-admin | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-in-admin | mobile | reading.init.daily_1: blocked (400). |
| logged-in-admin | mobile | reading.init.spread_3: created a2609bd4-44b1-4b40-a8bd-81b1980aac09. |
| logged-in-admin | mobile | reading.init.spread_5: created 0d01e4f9-623b-47ee-809c-5ec9d8edb6be. |
| logged-in-admin | mobile | reading.init.spread_10: created 18b9cb87-1bbd-4bb9-8b20-bdd137ccb0d9. |
| logged-in-admin | mobile | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-admin | mobile | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-admin | mobile | reader-detail:ui-discovery-empty |
| logged-in-admin | mobile | chat-room-detail:ui-discovery-empty |
| logged-in-admin | mobile | reading-history-detail:ui-discovery-empty |
| logged-in-admin | mobile | community-posts:api-discovery-1 |
| logged-in-admin | mobile | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-reader | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-reader | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-in-reader | mobile | reading.init.daily_1: blocked (400). |
| logged-in-reader | mobile | reading.init.spread_3: created 9f9f51dd-a2a1-4a56-a999-a748edc14160. |
| logged-in-reader | mobile | reading.init.spread_5: created b3592556-bd0f-4779-8274-a037973bd26e. |
| logged-in-reader | mobile | reading.init.spread_10: created d512f861-d1df-498b-99d1-d9eb0566f847. |
| logged-in-reader | mobile | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-reader | mobile | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-reader | mobile | reader-detail:ui-discovery-empty |
| logged-in-reader | mobile | chat-room-detail:ui-discovery-empty |
| logged-in-reader | mobile | reading-history-detail:ui-discovery-empty |
| logged-in-reader | mobile | community-posts:api-discovery-1 |
| logged-in-reader | mobile | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-reader | mobile | scenario-filter:reader-admin-routes-skipped=1 |

## Login Bootstrap Notes
### logged-in-admin / desktop
- Attempt 1: login response and route-change both failed.
- Attempt 2: login bootstrap succeeded.

### logged-in-reader / desktop
- Attempt 1: login bootstrap succeeded.

### logged-in-admin / mobile
- Attempt 1: login response and route-change both failed.
- Attempt 2: login bootstrap succeeded.

### logged-in-reader / mobile
- Attempt 1: login response and route-change both failed.
- Attempt 2: login bootstrap succeeded.
