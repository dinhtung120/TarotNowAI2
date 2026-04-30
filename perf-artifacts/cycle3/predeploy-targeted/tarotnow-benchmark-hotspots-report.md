# TarotNow Navigation Benchmark Report

- Generated at (UTC): 2026-04-30T06:15:45.063Z
- Base URL: https://www.tarotnow.xyz/vi
- Benchmark mode: targeted-hotspots
- Thresholds: >35 requests = Critical, >25 = High, request >800ms = High, >400ms = Medium
- Critical pages (request count): 22
- High pages (request count): 40
- High slow requests: 415
- Medium slow requests: 270

## Scenario Summary
| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Document Reloads | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 0 | 0 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 11 | 4322 | 573 | 0 | 1 | 3 | 2 | yes |
| logged-in-reader | desktop | 10 | 3911 | 539 | 16 | 1 | 3 | 3 | yes |
| logged-out | mobile | 0 | 0 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 10 | 5646 | 429 | 0 | 1 | 4 | 2 | yes |
| logged-in-reader | mobile | 9 | 4111 | 393 | 0 | 1 | 3 | 2 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 1 | 35.0 | 3775 | 0 |
| logged-in-admin | desktop | collection | 1 | 210.0 | 14908 | 0 |
| logged-in-admin | desktop | community | 1 | 43.0 | 3968 | 0 |
| logged-in-admin | desktop | readers | 3 | 33.0 | 3112 | 0 |
| logged-in-admin | desktop | reading | 4 | 38.3 | 3149 | 0 |
| logged-in-admin | desktop | wallet | 1 | 33.0 | 2963 | 0 |
| logged-in-admin | mobile | admin | 1 | 35.0 | 3516 | 0 |
| logged-in-admin | mobile | collection | 1 | 106.0 | 7925 | 0 |
| logged-in-admin | mobile | community | 1 | 43.0 | 3856 | 0 |
| logged-in-admin | mobile | readers | 3 | 33.7 | 9589 | 0 |
| logged-in-admin | mobile | reading | 3 | 37.0 | 3137 | 0 |
| logged-in-admin | mobile | wallet | 1 | 33.0 | 2988 | 0 |
| logged-in-reader | desktop | collection | 1 | 211.0 | 10265 | 16 |
| logged-in-reader | desktop | community | 1 | 44.0 | 4099 | 0 |
| logged-in-reader | desktop | readers | 3 | 33.0 | 2965 | 0 |
| logged-in-reader | desktop | reading | 4 | 38.0 | 3167 | 0 |
| logged-in-reader | desktop | wallet | 1 | 33.0 | 3182 | 0 |
| logged-in-reader | mobile | collection | 1 | 106.0 | 7916 | 0 |
| logged-in-reader | mobile | community | 1 | 43.0 | 6381 | 0 |
| logged-in-reader | mobile | readers | 3 | 33.3 | 3062 | 0 |
| logged-in-reader | mobile | reading | 3 | 37.0 | 3275 | 0 |
| logged-in-reader | mobile | wallet | 1 | 33.0 | 3691 | 0 |

## Per-Page Metrics
| Scenario | Viewport | Route | Requests | Severity | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Collection Img Requests | Collection Img 400-800ms | Collection Img >800ms | Collection Img First Load | Collection Img Reload | Collection Img Cache Hits | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |
| --- | --- | --- | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/admin | 35 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3775 | 1061 | 1715 | 1488 | 1488 | 0.0000 | 58.0 | 598085 |
| logged-in-admin | desktop | /vi/wallet/deposit/history | 33 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2963 | 863 | 876 | 1172 | 1468 | 0.0029 | 40.0 | 579837 |
| logged-in-admin | desktop | /vi/community | 43 | critical | 0 | 0 | 1 | 1 | 3 | 2 | 1 | 3 | 0 | 0 | 3968 | 870 | 1030 | 772 | 2180 | 0.0029 | 40.0 | 735330 |
| logged-in-admin | desktop | /vi/collection | 210 | critical | 1 | 0 | 1 | 1 | 142 | 1 | 140 | 71 | 71 | 0 | 14908 | 2875 | 4687 | 2288 | 3484 | 0.0029 | 859.0 | 25237410 |
| logged-in-admin | desktop | /vi/reading/session/d9205fd3-468c-49ac-9efd-b19f4f4f2bde | 38 | critical | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3024 | 848 | 999 | 1144 | 1144 | 0.0037 | 59.0 | 672220 |
| logged-in-admin | desktop | /vi/reading/session/1809335b-52c8-4c66-a529-3e4f7c384428 | 39 | critical | 0 | 0 | 1 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3241 | 1032 | 1216 | 1396 | 1396 | 0.0039 | 79.0 | 672930 |
| logged-in-admin | desktop | /vi/reading/session/4e103dcf-c93c-41ae-970c-677245b528bc | 38 | critical | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3116 | 962 | 1091 | 1220 | 1220 | 0.0029 | 54.0 | 672060 |
| logged-in-admin | desktop | /vi/reading/session/9ccd9a5a-8ece-405b-9227-13dd19760308 | 38 | critical | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3216 | 1012 | 1188 | 1388 | 1388 | 0.0037 | 75.0 | 672186 |
| logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 33 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3032 | 908 | 908 | 1320 | 1320 | 0.0029 | 25.0 | 578773 |
| logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2991 | 919 | 919 | 1260 | 1260 | 0.0029 | 22.0 | 578645 |
| logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 33 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3312 | 934 | 978 | 1328 | 1328 | 0.0029 | 29.0 | 578677 |
| logged-in-reader | desktop | /vi/wallet/deposit/history | 33 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3182 | 1103 | 1103 | 1512 | 1512 | 0.0026 | 35.0 | 579348 |
| logged-in-reader | desktop | /vi/community | 44 | critical | 0 | 0 | 1 | 2 | 4 | 1 | 0 | 4 | 0 | 0 | 4099 | 1031 | 1083 | 1324 | 2060 | 0.0026 | 56.0 | 735609 |
| logged-in-reader | desktop | /vi/collection | 211 | critical | 1 | 0 | 2 | 1 | 142 | 3 | 123 | 71 | 71 | 0 | 10265 | 1053 | 1935 | 1668 | 2400 | 0.0026 | 1088.0 | 22933152 |
| logged-in-reader | desktop | /vi/reading/session/40b32c07-4311-40b4-b7a2-ca62fea1da96 | 38 | critical | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3101 | 938 | 1077 | 1272 | 1272 | 0.0026 | 65.0 | 672233 |
| logged-in-reader | desktop | /vi/reading/session/65b23b34-6f38-497b-b6d1-3a9c58d5e9d2 | 38 | critical | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3180 | 1016 | 1157 | 1384 | 1384 | 0.0036 | 56.0 | 672176 |
| logged-in-reader | desktop | /vi/reading/session/c8026be2-c55b-4692-b014-3d9ecf3a67f0 | 38 | critical | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3404 | 1152 | 1180 | 1292 | 1524 | 0.0026 | 58.0 | 672259 |
| logged-in-reader | desktop | /vi/reading/session/4dc76421-fea5-4316-96bd-289ca23ce936 | 38 | critical | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2981 | 819 | 957 | 1124 | 1124 | 0.0026 | 57.0 | 672098 |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 33 | high | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 2873 | 797 | 811 | 1076 | 1076 | 0.0026 | 24.0 | 578695 |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2995 | 822 | 851 | 1204 | 1204 | 0.0026 | 23.0 | 578926 |
| logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 33 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3028 | 853 | 885 | 1212 | 1212 | 0.0026 | 25.0 | 578677 |
| logged-in-admin | mobile | /vi/admin | 35 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3516 | 918 | 962 | 1024 | 1024 | 0.0000 | 50.0 | 597910 |
| logged-in-admin | mobile | /vi/wallet/deposit/history | 33 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2988 | 926 | 940 | 996 | 996 | 0.0000 | 32.0 | 580701 |
| logged-in-admin | mobile | /vi/community | 43 | critical | 0 | 0 | 1 | 1 | 3 | 2 | 0 | 3 | 0 | 0 | 3856 | 946 | 948 | 916 | 1948 | 0.0000 | 39.0 | 736358 |
| logged-in-admin | mobile | /vi/collection | 106 | critical | 1 | 0 | 1 | 1 | 38 | 16 | 16 | 19 | 19 | 0 | 7925 | 1037 | 1594 | 900 | 1368 | 0.0030 | 132.0 | 8191635 |
| logged-in-admin | mobile | /vi/reading/session/155e08ac-c0ad-4250-b638-aee28e4e240e | 37 | critical | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3072 | 915 | 1036 | 1004 | 1004 | 0.0000 | 57.0 | 641247 |
| logged-in-admin | mobile | /vi/reading/session/bb74c041-b09f-4410-815c-28502e046fd7 | 37 | critical | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2978 | 837 | 954 | 940 | 940 | 0.0000 | 45.0 | 641156 |
| logged-in-admin | mobile | /vi/reading/session/a94d9f1f-1e47-4e06-801e-6e03566231a0 | 37 | critical | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3360 | 959 | 1138 | 1100 | 1100 | 0.0000 | 71.0 | 641386 |
| logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 34 | high | 0 | 0 | 1 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3097 | 958 | 1020 | 932 | 1320 | 0.0000 | 26.0 | 580908 |
| logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3006 | 931 | 931 | 1036 | 1036 | 0.0000 | 27.0 | 579855 |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 34 | high | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 22663 | 16697 | 16736 | 1548 | 1548 | 0.0030 | 376.0 | 581042 |
| logged-in-reader | mobile | /vi/wallet/deposit/history | 33 | high | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3691 | 1405 | 1436 | 1560 | 1560 | 0.0000 | 91.0 | 580533 |
| logged-in-reader | mobile | /vi/community | 43 | critical | 0 | 0 | 1 | 1 | 3 | 0 | 3 | 3 | 0 | 0 | 6381 | 938 | 947 | 988 | 3396 | 0.0051 | 216.0 | 736198 |
| logged-in-reader | mobile | /vi/collection | 106 | critical | 1 | 0 | 1 | 1 | 38 | 17 | 20 | 19 | 19 | 0 | 7916 | 1130 | 1944 | 992 | 1568 | 0.0029 | 242.0 | 8188604 |
| logged-in-reader | mobile | /vi/reading/session/404d85f3-9876-4b70-a859-be5c4bbf3589 | 37 | critical | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3285 | 960 | 1267 | 1060 | 1060 | 0.0000 | 72.0 | 641252 |
| logged-in-reader | mobile | /vi/reading/session/502d9f03-743c-491c-9594-96009c9f21c9 | 37 | critical | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3190 | 1012 | 1132 | 1100 | 1100 | 0.0000 | 58.0 | 641403 |
| logged-in-reader | mobile | /vi/reading/session/0511c82e-20f9-4b6d-828d-f3cb4dd78201 | 37 | critical | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3349 | 1121 | 1294 | 992 | 992 | 0.0000 | 78.0 | 641491 |
| logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 33 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3080 | 996 | 1048 | 1056 | 1056 | 0.0000 | 26.0 | 580048 |
| logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2998 | 876 | 907 | 968 | 968 | 0.0000 | 26.0 | 579792 |
| logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 34 | high | 0 | 0 | 1 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3109 | 839 | 873 | 804 | 1184 | 0.0000 | 25.0 | 580808 |

## Collection Image Focus
| Scenario | Viewport | Route | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reload Img Requests | 304 Cache Hits |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/collection | 142 | 1 | 140 | 71 | 71 | 0 |
| logged-in-reader | desktop | /vi/collection | 142 | 3 | 123 | 71 | 71 | 0 |
| logged-in-admin | mobile | /vi/collection | 38 | 16 | 16 | 19 | 19 | 0 |
| logged-in-reader | mobile | /vi/collection | 38 | 17 | 20 | 19 | 19 | 0 |

## Suspicious Pages (>25 requests)
| Scenario | Viewport | Route | Request Count | Severity | API | Static | Third-party |
| --- | --- | --- | ---: | --- | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/admin | 35 | high | 0 | 32 | 0 |
| logged-in-admin | desktop | /vi/wallet/deposit/history | 33 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | /vi/community | 43 | critical | 3 | 37 | 0 |
| logged-in-admin | desktop | /vi/collection | 210 | critical | 1 | 62 | 140 |
| logged-in-admin | desktop | /vi/reading/session/d9205fd3-468c-49ac-9efd-b19f4f4f2bde | 38 | critical | 1 | 34 | 0 |
| logged-in-admin | desktop | /vi/reading/session/1809335b-52c8-4c66-a529-3e4f7c384428 | 39 | critical | 2 | 34 | 0 |
| logged-in-admin | desktop | /vi/reading/session/4e103dcf-c93c-41ae-970c-677245b528bc | 38 | critical | 1 | 34 | 0 |
| logged-in-admin | desktop | /vi/reading/session/9ccd9a5a-8ece-405b-9227-13dd19760308 | 38 | critical | 1 | 34 | 0 |
| logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 33 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 33 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi/wallet/deposit/history | 33 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi/community | 44 | critical | 3 | 38 | 0 |
| logged-in-reader | desktop | /vi/collection | 211 | critical | 2 | 62 | 140 |
| logged-in-reader | desktop | /vi/reading/session/40b32c07-4311-40b4-b7a2-ca62fea1da96 | 38 | critical | 1 | 34 | 0 |
| logged-in-reader | desktop | /vi/reading/session/65b23b34-6f38-497b-b6d1-3a9c58d5e9d2 | 38 | critical | 1 | 34 | 0 |
| logged-in-reader | desktop | /vi/reading/session/c8026be2-c55b-4692-b014-3d9ecf3a67f0 | 38 | critical | 1 | 34 | 0 |
| logged-in-reader | desktop | /vi/reading/session/4dc76421-fea5-4316-96bd-289ca23ce936 | 38 | critical | 1 | 34 | 0 |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 33 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 33 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | /vi/admin | 35 | high | 0 | 32 | 0 |
| logged-in-admin | mobile | /vi/wallet/deposit/history | 33 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | /vi/community | 43 | critical | 3 | 37 | 0 |
| logged-in-admin | mobile | /vi/collection | 106 | critical | 1 | 62 | 36 |
| logged-in-admin | mobile | /vi/reading/session/155e08ac-c0ad-4250-b638-aee28e4e240e | 37 | critical | 1 | 33 | 0 |
| logged-in-admin | mobile | /vi/reading/session/bb74c041-b09f-4410-815c-28502e046fd7 | 37 | critical | 1 | 33 | 0 |
| logged-in-admin | mobile | /vi/reading/session/a94d9f1f-1e47-4e06-801e-6e03566231a0 | 37 | critical | 1 | 33 | 0 |
| logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 34 | high | 1 | 30 | 0 |
| logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 34 | high | 1 | 30 | 0 |
| logged-in-reader | mobile | /vi/wallet/deposit/history | 33 | high | 0 | 30 | 0 |
| logged-in-reader | mobile | /vi/community | 43 | critical | 3 | 37 | 0 |
| logged-in-reader | mobile | /vi/collection | 106 | critical | 1 | 62 | 36 |
| logged-in-reader | mobile | /vi/reading/session/404d85f3-9876-4b70-a859-be5c4bbf3589 | 37 | critical | 1 | 33 | 0 |
| logged-in-reader | mobile | /vi/reading/session/502d9f03-743c-491c-9594-96009c9f21c9 | 37 | critical | 1 | 33 | 0 |
| logged-in-reader | mobile | /vi/reading/session/0511c82e-20f9-4b6d-828d-f3cb4dd78201 | 37 | critical | 1 | 33 | 0 |
| logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 33 | high | 0 | 30 | 0 |
| logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 0 | 30 | 0 |
| logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 34 | high | 1 | 30 | 0 |

## High Slow Requests (>800ms)
| Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 18747 | 91 | static | https://www.tarotnow.xyz/_next/static/chunks/12gvu2nm._6pa.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 18697 | 78 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 18696 | 71 | static | https://www.tarotnow.xyz/_next/static/chunks/0r68tfefeu~dz.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 18516 | 76 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 18450 | 83 | static | https://www.tarotnow.xyz/_next/static/chunks/04bi685.wk-4b.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 18441 | 323 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 17816 | 73 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 17766 | 76 | static | https://www.tarotnow.xyz/_next/static/chunks/07cbcba783g-t.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 17577 | 76 | static | https://www.tarotnow.xyz/_next/static/chunks/09njxa758vvw_.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 17565 | 72 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 17430 | 112 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 17361 | 73 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-0-hlnlst9psev.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 17280 | 97 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 17164 | 78 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 17157 | 81 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 16934 | 128 | static | https://www.tarotnow.xyz/_next/static/chunks/010jrdgfxuf04.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 16818 | 137 | static | https://www.tarotnow.xyz/_next/static/chunks/07tk2ft0d9n3x.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 16781 | 178 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 16693 | 131 | static | https://www.tarotnow.xyz/_next/static/chunks/0rvsgo27-na.8.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 16651 | 75 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg6ntv_3jdd4.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 16464 | 319 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 16333 | 73 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 16295 | 74 | static | https://www.tarotnow.xyz/_next/static/chunks/0fqgq6m2b-440.css |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 16240 | 69 | static | https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 13967 | 135 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 7702 | 71 | static | https://www.tarotnow.xyz/_next/static/chunks/0k2ye7~105rps.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 4006 | 75 | static | https://www.tarotnow.xyz/_next/static/chunks/00ny0go~4bchx.js |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3876 | 151 | third-party | https://img.tarotnow.xyz/light-god-50/42_Six_of_Pentacles_50_20260325_181413.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3866 | 197 | third-party | https://img.tarotnow.xyz/light-god-50/41_Five_of_Pentacles_50_20260325_181413.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3801 | 205 | third-party | https://img.tarotnow.xyz/light-god-50/40_Four_of_Pentacles_50_20260325_181413.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3749 | 150 | third-party | https://img.tarotnow.xyz/light-god-50/63_Queen_of_Swords_50_20260325_181427.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 3718 | 156 | third-party | https://img.tarotnow.xyz/light-god-50/42_Six_of_Pentacles_50_20260325_181413.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3706 | 150 | third-party | https://img.tarotnow.xyz/light-god-50/62_Knight_of_Swords_50_20260325_181427.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3705 | 197 | third-party | https://img.tarotnow.xyz/light-god-50/39_Three_of_Pentacles_50_20260325_181411.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3698 | 175 | third-party | https://img.tarotnow.xyz/light-god-50/64_King_of_Swords_50_20260325_181428.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3670 | 178 | third-party | https://img.tarotnow.xyz/light-god-50/38_Two_of_Pentacles_50_20260325_181411.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 3670 | 403 | third-party | https://img.tarotnow.xyz/light-god-50/41_Five_of_Pentacles_50_20260325_181413.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 3666 | 167 | third-party | https://img.tarotnow.xyz/light-god-50/64_King_of_Swords_50_20260325_181428.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3663 | 164 | third-party | https://img.tarotnow.xyz/light-god-50/37_Ace_of_Pentacles_50_20260325_181411.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 3662 | 434 | third-party | https://img.tarotnow.xyz/light-god-50/40_Four_of_Pentacles_50_20260325_181413.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 3648 | 542 | third-party | https://img.tarotnow.xyz/light-god-50/37_Ace_of_Pentacles_50_20260325_181411.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 3645 | 493 | third-party | https://img.tarotnow.xyz/light-god-50/39_Three_of_Pentacles_50_20260325_181411.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 3639 | 540 | third-party | https://img.tarotnow.xyz/light-god-50/38_Two_of_Pentacles_50_20260325_181411.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3577 | 115 | third-party | https://img.tarotnow.xyz/light-god-50/61_Page_of_Swords_50_20260325_181427.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3565 | 144 | third-party | https://img.tarotnow.xyz/light-god-50/60_Ten_of_Swords_50_20260325_181426.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3558 | 78 | third-party | https://img.tarotnow.xyz/light-god-50/58_Eight_of_Swords_50_20260325_181426.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3558 | 78 | third-party | https://img.tarotnow.xyz/light-god-50/59_Nine_of_Swords_50_20260325_181426.avif |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 3525 | 94 | static | https://www.tarotnow.xyz/_next/static/chunks/0a7-atv09sxt4.js |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3517 | 78 | third-party | https://img.tarotnow.xyz/light-god-50/57_Seven_of_Swords_50_20260325_181424.avif |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 3503 | 93 | static | https://www.tarotnow.xyz/_next/static/chunks/08nnjyw~vjmez.js |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3375 | 272 | third-party | https://img.tarotnow.xyz/light-god-50/42_Six_of_Pentacles_50_20260325_181413.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 3373 | 176 | third-party | https://img.tarotnow.xyz/light-god-50/62_Knight_of_Swords_50_20260325_181427.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3358 | 218 | third-party | https://img.tarotnow.xyz/light-god-50/41_Five_of_Pentacles_50_20260325_181413.avif |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 3358 | 91 | static | https://www.tarotnow.xyz/_next/static/chunks/14zfgnebl8n68.js |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3343 | 70 | third-party | https://img.tarotnow.xyz/light-god-50/56_Six_of_Swords_50_20260325_181424.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 3325 | 174 | third-party | https://img.tarotnow.xyz/light-god-50/61_Page_of_Swords_50_20260325_181427.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 3317 | 182 | third-party | https://img.tarotnow.xyz/light-god-50/63_Queen_of_Swords_50_20260325_181427.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 3300 | 233 | third-party | https://img.tarotnow.xyz/light-god-50/59_Nine_of_Swords_50_20260325_181426.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 3300 | 185 | third-party | https://img.tarotnow.xyz/light-god-50/60_Ten_of_Swords_50_20260325_181426.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3295 | 53 | third-party | https://img.tarotnow.xyz/light-god-50/54_Four_of_Swords_50_20260325_181422.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3292 | 70 | third-party | https://img.tarotnow.xyz/light-god-50/55_Five_of_Swords_50_20260325_181424.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3291 | 194 | third-party | https://img.tarotnow.xyz/light-god-50/40_Four_of_Pentacles_50_20260325_181413.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 3278 | 233 | third-party | https://img.tarotnow.xyz/light-god-50/58_Eight_of_Swords_50_20260325_181426.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 3262 | 194 | third-party | https://img.tarotnow.xyz/light-god-50/57_Seven_of_Swords_50_20260325_181424.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3248 | 561 | third-party | https://img.tarotnow.xyz/light-god-50/51_Ace_of_Swords_50_20260325_181419.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3242 | 516 | third-party | https://img.tarotnow.xyz/light-god-50/52_Two_of_Swords_50_20260325_181422.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3237 | 169 | third-party | https://img.tarotnow.xyz/light-god-50/39_Three_of_Pentacles_50_20260325_181411.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3236 | 487 | third-party | https://img.tarotnow.xyz/light-god-50/53_Three_of_Swords_50_20260325_181422.avif |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 3223 | 49 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3210 | 388 | third-party | https://img.tarotnow.xyz/light-god-50/36_King_of_Cups_50_20260325_181408.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3205 | 388 | third-party | https://img.tarotnow.xyz/light-god-50/35_Queen_of_Cups_50_20260325_181408.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3187 | 293 | third-party | https://img.tarotnow.xyz/light-god-50/34_Knight_of_Cups_50_20260325_181408.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3182 | 138 | third-party | https://img.tarotnow.xyz/light-god-50/54_Four_of_Swords_50_20260325_181422.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 3177 | 239 | third-party | https://img.tarotnow.xyz/light-god-50/56_Six_of_Swords_50_20260325_181424.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 3173 | 238 | third-party | https://img.tarotnow.xyz/light-god-50/55_Five_of_Swords_50_20260325_181424.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 3168 | 264 | third-party | https://img.tarotnow.xyz/light-god-50/54_Four_of_Swords_50_20260325_181422.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3139 | 122 | third-party | https://img.tarotnow.xyz/light-god-50/53_Three_of_Swords_50_20260325_181422.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3134 | 224 | third-party | https://img.tarotnow.xyz/light-god-50/38_Two_of_Pentacles_50_20260325_181411.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3128 | 75 | third-party | https://img.tarotnow.xyz/light-god-50/52_Two_of_Swords_50_20260325_181422.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3114 | 166 | third-party | https://img.tarotnow.xyz/light-god-50/51_Ace_of_Swords_50_20260325_181419.avif |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-reader | mobile | /vi/reading/session/502d9f03-743c-491c-9594-96009c9f21c9 | GET | 200 | 799 | 354 | html | https://www.tarotnow.xyz/vi/reading/session/502d9f03-743c-491c-9594-96009c9f21c9 |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 794 | 602 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=32&q=75 |
| logged-in-admin | desktop | /vi/admin | GET | 200 | 789 | 253 | static | https://www.tarotnow.xyz/_next/static/chunks/07o~3chaybnsv.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 786 | 315 | api | https://www.tarotnow.xyz/api/auth/session |
| logged-in-admin | mobile | /vi/community | GET | 200 | 783 | 300 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-in-reader | mobile | /vi/collection | GET | 200 | 780 | 110 | third-party | https://img.tarotnow.xyz/light-god-50/03_The_High%20Priestess%20_50_20260325_181348.avif |
| logged-in-admin | mobile | /vi/admin | GET | 200 | 774 | 320 | html | https://www.tarotnow.xyz/vi/admin |
| logged-in-admin | desktop | /vi/community | GET | 200 | 773 | 323 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | desktop | /vi/reading/session/c8026be2-c55b-4692-b014-3d9ecf3a67f0 | GET | 200 | 771 | 298 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | desktop | /vi/community | GET | 200 | 767 | 297 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 767 | 363 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | mobile | /vi/collection | GET | 200 | 764 | 123 | third-party | https://img.tarotnow.xyz/light-god-50/10_The_Hermit_50_20260325_181353.avif |
| logged-in-reader | mobile | /vi/collection | GET | 200 | 760 | 149 | third-party | https://img.tarotnow.xyz/light-god-50/09_Strength_50_20260325_181351.avif |
| logged-in-admin | desktop | /vi/admin | GET | 200 | 755 | 284 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | /vi/collection | GET | 200 | 755 | 354 | third-party | https://img.tarotnow.xyz/light-god-50/07_The_Lovers_50_20260325_181351.avif |
| logged-in-admin | desktop | /vi/admin | GET | 200 | 749 | 317 | static | https://www.tarotnow.xyz/_next/static/chunks/08nnjyw~vjmez.js |
| logged-in-reader | mobile | /vi/collection | GET | 200 | 740 | 129 | third-party | https://img.tarotnow.xyz/light-god-50/04_The_Empress_50_20260325_181348.avif |
| logged-in-reader | mobile | /vi/collection | GET | 200 | 738 | 129 | third-party | https://img.tarotnow.xyz/light-god-50/05_The_Emperor_50_20260325_181348.avif |
| logged-in-reader | mobile | /vi/collection | GET | 200 | 737 | 110 | third-party | https://img.tarotnow.xyz/light-god-50/01_The_Fool_50_20260325_181348.avif |
| logged-in-reader | mobile | /vi/collection | GET | 200 | 731 | 159 | third-party | https://img.tarotnow.xyz/light-god-50/08_The_Chariot_50_20260325_181351.avif |
| logged-in-admin | mobile | /vi/collection | GET | 200 | 730 | 130 | third-party | https://img.tarotnow.xyz/light-god-50/18_The_Star_50_20260325_181357.avif |
| logged-in-admin | mobile | /vi/admin | GET | 200 | 726 | 99 | static | https://www.tarotnow.xyz/_next/static/chunks/00ny0go~4bchx.js |
| logged-in-admin | mobile | /vi/admin | GET | 200 | 726 | 119 | static | https://www.tarotnow.xyz/_next/static/chunks/08nnjyw~vjmez.js |
| logged-in-admin | mobile | /vi/reading/session/a94d9f1f-1e47-4e06-801e-6e03566231a0 | GET | 200 | 726 | 318 | html | https://www.tarotnow.xyz/vi/reading/session/a94d9f1f-1e47-4e06-801e-6e03566231a0 |
| logged-in-reader | mobile | /vi/collection | GET | 200 | 726 | 116 | third-party | https://img.tarotnow.xyz/light-god-50/06_The_Hierophant_50_20260325_181348.avif |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 724 | 298 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=96&q=75 |
| logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 721 | 301 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | desktop | /vi/reading/session/40b32c07-4311-40b4-b7a2-ca62fea1da96 | GET | 200 | 719 | 365 | html | https://www.tarotnow.xyz/vi/reading/session/40b32c07-4311-40b4-b7a2-ca62fea1da96 |
| logged-in-admin | mobile | /vi/collection | GET | 200 | 718 | 118 | third-party | https://img.tarotnow.xyz/light-god-50/17_The_Tower_50_20260325_181357.avif |
| logged-in-admin | desktop | /vi/admin | GET | 200 | 715 | 253 | static | https://www.tarotnow.xyz/_next/static/chunks/1405uwf5x5tos.js |
| logged-in-admin | mobile | /vi/admin | GET | 200 | 713 | 106 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | mobile | /vi/community | GET | 200 | 713 | 327 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | mobile | /vi/admin | GET | 200 | 708 | 89 | static | https://www.tarotnow.xyz/_next/static/chunks/12gvu2nm._6pa.js |
| logged-in-reader | desktop | /vi/community | GET | 200 | 706 | 391 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 704 | 424 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-admin | mobile | /vi/admin | GET | 200 | 703 | 84 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | /vi/admin | GET | 200 | 703 | 113 | static | https://www.tarotnow.xyz/_next/static/chunks/0a_d5lq~~abs2.js |
| logged-in-reader | mobile | /vi/collection | GET | 200 | 702 | 568 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=96&q=75 |
| logged-in-admin | mobile | /vi/admin | GET | 200 | 700 | 136 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | mobile | /vi/admin | GET | 200 | 699 | 119 | static | https://www.tarotnow.xyz/_next/static/chunks/14b2~q6psb_mt.js |
| logged-in-admin | mobile | /vi/community | GET | 200 | 699 | 382 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | mobile | /vi/admin | GET | 200 | 698 | 116 | static | https://www.tarotnow.xyz/_next/static/chunks/0ccdh6h.1j6oy.js |
| logged-in-admin | desktop | /vi/admin | GET | 200 | 697 | 347 | html | https://www.tarotnow.xyz/vi/admin |
| logged-in-reader | desktop | /vi/reading/session/65b23b34-6f38-497b-b6d1-3a9c58d5e9d2 | GET | 200 | 696 | 409 | html | https://www.tarotnow.xyz/vi/reading/session/65b23b34-6f38-497b-b6d1-3a9c58d5e9d2 |
| logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 695 | 340 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 693 | 309 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | mobile | /vi/admin | GET | 200 | 693 | 88 | static | https://www.tarotnow.xyz/_next/static/chunks/0r68tfefeu~dz.js |
| logged-in-admin | mobile | /vi/collection | GET | 200 | 690 | 395 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | desktop | /vi/wallet/deposit/history | GET | 200 | 689 | 340 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-reader | desktop | /vi/reading/session/c8026be2-c55b-4692-b014-3d9ecf3a67f0 | GET | 200 | 689 | 134 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-0-hlnlst9psev.js |
| logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 686 | 339 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-reader | mobile | /vi/reading/session/0511c82e-20f9-4b6d-828d-f3cb4dd78201 | GET | 200 | 686 | 301 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | mobile | /vi/reading/session/0511c82e-20f9-4b6d-828d-f3cb4dd78201 | GET | 200 | 685 | 247 | static | https://www.tarotnow.xyz/_next/static/chunks/00ny0go~4bchx.js |
| logged-in-admin | mobile | /vi/wallet/deposit/history | GET | 200 | 684 | 324 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-reader | mobile | /vi/community | GET | 200 | 684 | 79 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-reader | mobile | /vi/reading/session/0511c82e-20f9-4b6d-828d-f3cb4dd78201 | GET | 200 | 683 | 262 | static | https://www.tarotnow.xyz/_next/static/chunks/0a7-atv09sxt4.js |
| logged-in-reader | desktop | /vi/reading/session/c8026be2-c55b-4692-b014-3d9ecf3a67f0 | GET | 200 | 681 | 135 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | mobile | /vi/admin | GET | 200 | 679 | 93 | static | https://www.tarotnow.xyz/_next/static/chunks/0ar0f-egwe8w~.js |
| logged-in-admin | mobile | /vi/collection | GET | 200 | 677 | 125 | third-party | https://img.tarotnow.xyz/light-god-50/16_The_Devil_50_20260325_181357.avif |
| logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 676 | 331 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-reader | mobile | /vi/collection | GET | 200 | 673 | 488 | api | https://www.tarotnow.xyz/api/auth/session |
| logged-in-reader | mobile | /vi/reading/session/0511c82e-20f9-4b6d-828d-f3cb4dd78201 | GET | 200 | 668 | 252 | static | https://www.tarotnow.xyz/_next/static/chunks/08nnjyw~vjmez.js |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 665 | 76 | third-party | https://img.tarotnow.xyz/light-god-50/01_The_Fool_50_20260325_181348.avif |
| logged-in-admin | mobile | /vi/reading/session/155e08ac-c0ad-4250-b638-aee28e4e240e | GET | 200 | 665 | 385 | html | https://www.tarotnow.xyz/vi/reading/session/155e08ac-c0ad-4250-b638-aee28e4e240e |
| logged-in-admin | desktop | /vi/reading/session/d9205fd3-468c-49ac-9efd-b19f4f4f2bde | GET | 200 | 664 | 332 | html | https://www.tarotnow.xyz/vi/reading/session/d9205fd3-468c-49ac-9efd-b19f4f4f2bde |
| logged-in-reader | desktop | /vi/reading/session/c8026be2-c55b-4692-b014-3d9ecf3a67f0 | GET | 200 | 664 | 72 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | /vi/admin | GET | 200 | 663 | 88 | static | https://www.tarotnow.xyz/_next/static/chunks/010jrdgfxuf04.js |
| logged-in-reader | desktop | /vi/reading/session/c8026be2-c55b-4692-b014-3d9ecf3a67f0 | GET | 200 | 659 | 130 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 658 | 313 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 657 | 362 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-admin | mobile | /vi/reading/session/bb74c041-b09f-4410-815c-28502e046fd7 | GET | 200 | 656 | 317 | api | https://www.tarotnow.xyz/api/reading/cards-catalog |
| logged-in-reader | desktop | /vi/wallet/deposit/history | GET | 200 | 655 | 422 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-reader | desktop | /vi/reading/session/c8026be2-c55b-4692-b014-3d9ecf3a67f0 | GET | 200 | 655 | 84 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | mobile | /vi/admin | GET | 200 | 649 | 84 | static | https://www.tarotnow.xyz/_next/static/chunks/07o~3chaybnsv.js |
| logged-in-admin | desktop | /vi/reading/session/9ccd9a5a-8ece-405b-9227-13dd19760308 | GET | 200 | 647 | 363 | html | https://www.tarotnow.xyz/vi/reading/session/9ccd9a5a-8ece-405b-9227-13dd19760308 |
| logged-in-admin | mobile | /vi/collection | GET | 200 | 647 | 119 | third-party | https://img.tarotnow.xyz/light-god-50/15_Temperance_50_20260325_181356.avif |
| logged-in-reader | desktop | /vi/reading/session/4dc76421-fea5-4316-96bd-289ca23ce936 | GET | 200 | 646 | 309 | html | https://www.tarotnow.xyz/vi/reading/session/4dc76421-fea5-4316-96bd-289ca23ce936 |
| logged-in-admin | mobile | /vi/admin | GET | 200 | 644 | 79 | static | https://www.tarotnow.xyz/_next/static/chunks/1405uwf5x5tos.js |
| logged-in-admin | mobile | /vi/collection | GET | 200 | 641 | 304 | third-party | https://img.tarotnow.xyz/light-god-50/07_The_Lovers_50_20260325_181351.avif |
| logged-in-admin | mobile | /vi/admin | GET | 200 | 639 | 88 | static | https://www.tarotnow.xyz/_next/static/chunks/06acg0ontlo6-.js |

## Pending Requests
| Scenario | Viewport | Route | URL |
| --- | --- | --- | --- |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/55_Five_of_Swords_50_20260325_181424.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/56_Six_of_Swords_50_20260325_181424.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/57_Seven_of_Swords_50_20260325_181424.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/58_Eight_of_Swords_50_20260325_181426.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/59_Nine_of_Swords_50_20260325_181426.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/60_Ten_of_Swords_50_20260325_181426.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/61_Page_of_Swords_50_20260325_181427.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/62_Knight_of_Swords_50_20260325_181427.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/63_Queen_of_Swords_50_20260325_181427.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/64_King_of_Swords_50_20260325_181428.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/37_Ace_of_Pentacles_50_20260325_181411.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/38_Two_of_Pentacles_50_20260325_181411.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/39_Three_of_Pentacles_50_20260325_181411.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/40_Four_of_Pentacles_50_20260325_181413.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/41_Five_of_Pentacles_50_20260325_181413.avif |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/42_Six_of_Pentacles_50_20260325_181413.avif |

## Coverage Notes
| Scenario | Viewport | Note |
| --- | --- | --- |
| logged-out | desktop | origin-discovery:/sitemap.xml:routes-13 |
| logged-out | desktop | origin-discovery:/robots.txt:routes-13 |
| logged-out | desktop | dynamic-routes: skipped for logged-out scenario. |
| logged-out | desktop | scenario-filter:logged-out-protected-routes-skipped=4 |
| logged-in-admin | desktop | origin-discovery:/sitemap.xml:routes-13 |
| logged-in-admin | desktop | origin-discovery:/robots.txt:routes-13 |
| logged-in-admin | desktop | reading.init.daily_1: created d9205fd3-468c-49ac-9efd-b19f4f4f2bde. |
| logged-in-admin | desktop | reading.init.spread_3: created 1809335b-52c8-4c66-a529-3e4f7c384428. |
| logged-in-admin | desktop | reading.init.spread_5: created 4e103dcf-c93c-41ae-970c-677245b528bc. |
| logged-in-admin | desktop | reading.init.spread_10: created 9ccd9a5a-8ece-405b-9227-13dd19760308. |
| logged-in-admin | desktop | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-admin | desktop | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-admin | desktop | reader-detail:ui-discovery-empty |
| logged-in-admin | desktop | chat-room-detail:ui-discovery-empty |
| logged-in-admin | desktop | reading-history-detail:ui-discovery-empty |
| logged-in-admin | desktop | community-posts:api-discovery-1 |
| logged-in-admin | desktop | community-post-detail:69db54fc297f66f734421a3c:200 |
| logged-in-reader | desktop | origin-discovery:/sitemap.xml:routes-13 |
| logged-in-reader | desktop | origin-discovery:/robots.txt:routes-13 |
| logged-in-reader | desktop | reading.init.daily_1: created 40b32c07-4311-40b4-b7a2-ca62fea1da96. |
| logged-in-reader | desktop | reading.init.spread_3: created 65b23b34-6f38-497b-b6d1-3a9c58d5e9d2. |
| logged-in-reader | desktop | reading.init.spread_5: created c8026be2-c55b-4692-b014-3d9ecf3a67f0. |
| logged-in-reader | desktop | reading.init.spread_10: created 4dc76421-fea5-4316-96bd-289ca23ce936. |
| logged-in-reader | desktop | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-reader | desktop | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-reader | desktop | reader-detail:ui-discovery-empty |
| logged-in-reader | desktop | chat-room-detail:ui-discovery-empty |
| logged-in-reader | desktop | reading-history-detail:ui-discovery-empty |
| logged-in-reader | desktop | community-posts:api-discovery-1 |
| logged-in-reader | desktop | community-post-detail:69db54fc297f66f734421a3c:200 |
| logged-in-reader | desktop | scenario-filter:reader-admin-routes-skipped=1 |
| logged-out | mobile | origin-discovery:/sitemap.xml:routes-13 |
| logged-out | mobile | origin-discovery:/robots.txt:routes-13 |
| logged-out | mobile | dynamic-routes: skipped for logged-out scenario. |
| logged-out | mobile | scenario-filter:logged-out-protected-routes-skipped=4 |
| logged-in-admin | mobile | origin-discovery:/sitemap.xml:routes-13 |
| logged-in-admin | mobile | origin-discovery:/robots.txt:routes-13 |
| logged-in-admin | mobile | reading.init.daily_1: blocked (400). |
| logged-in-admin | mobile | reading.init.spread_3: created 155e08ac-c0ad-4250-b638-aee28e4e240e. |
| logged-in-admin | mobile | reading.init.spread_5: created bb74c041-b09f-4410-815c-28502e046fd7. |
| logged-in-admin | mobile | reading.init.spread_10: created a94d9f1f-1e47-4e06-801e-6e03566231a0. |
| logged-in-admin | mobile | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-admin | mobile | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-admin | mobile | reader-detail:ui-discovery-empty |
| logged-in-admin | mobile | chat-room-detail:ui-discovery-empty |
| logged-in-admin | mobile | reading-history-detail:ui-discovery-empty |
| logged-in-admin | mobile | community-posts:api-discovery-1 |
| logged-in-admin | mobile | community-post-detail:69db54fc297f66f734421a3c:200 |
| logged-in-reader | mobile | origin-discovery:/sitemap.xml:routes-13 |
| logged-in-reader | mobile | origin-discovery:/robots.txt:routes-13 |
| logged-in-reader | mobile | reading.init.daily_1: blocked (400). |
| logged-in-reader | mobile | reading.init.spread_3: created 404d85f3-9876-4b70-a859-be5c4bbf3589. |
| logged-in-reader | mobile | reading.init.spread_5: created 502d9f03-743c-491c-9594-96009c9f21c9. |
| logged-in-reader | mobile | reading.init.spread_10: created 0511c82e-20f9-4b6d-828d-f3cb4dd78201. |
| logged-in-reader | mobile | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-reader | mobile | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-reader | mobile | reader-detail:ui-discovery-empty |
| logged-in-reader | mobile | chat-room-detail:ui-discovery-empty |
| logged-in-reader | mobile | reading-history-detail:ui-discovery-empty |
| logged-in-reader | mobile | community-posts:api-discovery-1 |
| logged-in-reader | mobile | community-post-detail:69db54fc297f66f734421a3c:200 |
| logged-in-reader | mobile | scenario-filter:reader-admin-routes-skipped=1 |

## Login Bootstrap Notes
### logged-in-admin / desktop
- Attempt 1: login bootstrap succeeded.

### logged-in-reader / desktop
- Attempt 1: login bootstrap succeeded.

### logged-in-admin / mobile
- Attempt 1: login bootstrap succeeded.

### logged-in-reader / mobile
- Attempt 1: login bootstrap succeeded.
