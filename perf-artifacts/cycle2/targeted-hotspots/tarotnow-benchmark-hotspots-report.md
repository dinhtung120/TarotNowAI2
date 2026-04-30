# TarotNow Navigation Benchmark Report

- Generated at (UTC): 2026-04-29T21:00:33.240Z
- Base URL: https://www.tarotnow.xyz/vi
- Benchmark mode: targeted-hotspots
- Thresholds: >35 requests = Critical, >25 = High, request >800ms = High, >400ms = Medium
- Critical pages (request count): 20
- High pages (request count): 38
- High slow requests: 189
- Medium slow requests: 219

## Scenario Summary
| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Document Reloads | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 0 | 0 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 10 | 3632 | 430 | 0 | 0 | 3 | 1 | yes |
| logged-in-reader | desktop | 9 | 3945 | 395 | 17 | 0 | 3 | 1 | yes |
| logged-out | mobile | 0 | 0 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 10 | 3211 | 374 | 0 | 0 | 2 | 1 | yes |
| logged-in-reader | mobile | 9 | 3265 | 341 | 0 | 0 | 4 | 1 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 1 | 35.0 | 3197 | 0 |
| logged-in-admin | desktop | collection | 1 | 105.0 | 7574 | 0 |
| logged-in-admin | desktop | community | 1 | 44.0 | 4194 | 0 |
| logged-in-admin | desktop | readers | 3 | 33.0 | 3006 | 0 |
| logged-in-admin | desktop | reading | 3 | 38.0 | 3007 | 0 |
| logged-in-admin | desktop | wallet | 1 | 33.0 | 3317 | 0 |
| logged-in-admin | mobile | admin | 1 | 35.0 | 3102 | 0 |
| logged-in-admin | mobile | collection | 1 | 53.0 | 4063 | 0 |
| logged-in-admin | mobile | community | 1 | 43.0 | 3917 | 0 |
| logged-in-admin | mobile | readers | 3 | 33.0 | 3096 | 0 |
| logged-in-admin | mobile | reading | 3 | 37.0 | 2936 | 0 |
| logged-in-admin | mobile | wallet | 1 | 33.0 | 2934 | 0 |
| logged-in-reader | desktop | collection | 1 | 104.0 | 4185 | 17 |
| logged-in-reader | desktop | community | 1 | 44.0 | 4199 | 0 |
| logged-in-reader | desktop | readers | 3 | 33.3 | 3610 | 0 |
| logged-in-reader | desktop | reading | 3 | 38.0 | 4349 | 0 |
| logged-in-reader | desktop | wallet | 1 | 33.0 | 3240 | 0 |
| logged-in-reader | mobile | collection | 1 | 53.0 | 4506 | 0 |
| logged-in-reader | mobile | community | 1 | 43.0 | 4154 | 0 |
| logged-in-reader | mobile | readers | 3 | 33.3 | 2918 | 0 |
| logged-in-reader | mobile | reading | 3 | 37.0 | 2946 | 0 |
| logged-in-reader | mobile | wallet | 1 | 34.0 | 3134 | 0 |

## Per-Page Metrics
| Scenario | Viewport | Route | Requests | Severity | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |
| --- | --- | --- | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/admin | 35 | high | 0 | 0 | 0 | 0 | 3197 | 952 | 1126 | 1240 | 1240 | 0.0000 | 18.0 | 597434 |
| logged-in-admin | desktop | /vi/wallet/deposit/history | 33 | high | 0 | 0 | 0 | 0 | 3317 | 896 | 911 | 1316 | 1592 | 0.0029 | 102.0 | 579513 |
| logged-in-admin | desktop | /vi/community | 44 | critical | 0 | 0 | 2 | 1 | 4194 | 932 | 1109 | 1216 | 1980 | 0.0029 | 86.0 | 735840 |
| logged-in-admin | desktop | /vi/collection | 105 | critical | 0 | 0 | 1 | 0 | 7574 | 1906 | 5502 | 1020 | 2652 | 0.0029 | 347.0 | 12618945 |
| logged-in-admin | desktop | /vi/reading/session/06513c39-08d1-416a-82af-e258832c0a5d | 38 | critical | 0 | 0 | 0 | 0 | 2933 | 771 | 903 | 1036 | 1036 | 0.0039 | 0.0 | 671811 |
| logged-in-admin | desktop | /vi/reading/session/8a78f474-c140-476e-aba3-201e40196e9c | 38 | critical | 0 | 0 | 0 | 0 | 2919 | 772 | 882 | 1024 | 1024 | 0.0037 | 1.0 | 671892 |
| logged-in-admin | desktop | /vi/reading/session/3f88391a-1ef5-4e45-9ab6-1d42268ad9ca | 38 | critical | 0 | 0 | 0 | 0 | 3170 | 934 | 1134 | 1252 | 1252 | 0.0037 | 48.0 | 671848 |
| logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 33 | high | 0 | 0 | 0 | 0 | 3072 | 918 | 939 | 1128 | 1128 | 0.0029 | 2.0 | 578576 |
| logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 0 | 0 | 0 | 0 | 2954 | 846 | 866 | 1032 | 1032 | 0.0029 | 0.0 | 578409 |
| logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 33 | high | 0 | 0 | 0 | 0 | 2993 | 800 | 819 | 1116 | 1116 | 0.0029 | 0.0 | 578794 |
| logged-in-reader | desktop | /vi/wallet/deposit/history | 33 | high | 0 | 0 | 0 | 0 | 3240 | 1120 | 1143 | 1164 | 1380 | 0.0026 | 15.0 | 579458 |
| logged-in-reader | desktop | /vi/community | 44 | critical | 0 | 0 | 2 | 1 | 4199 | 877 | 1173 | 1136 | 2144 | 0.0026 | 43.0 | 736489 |
| logged-in-reader | desktop | /vi/collection | 104 | critical | 0 | 0 | 0 | 0 | 4185 | 1132 | 2133 | 1200 | 2192 | 0.0026 | 178.0 | 10129667 |
| logged-in-reader | desktop | /vi/reading/session/e733f944-ccf4-4950-9ec5-a78e80cdc2e7 | 38 | critical | 0 | 0 | 0 | 0 | 2998 | 813 | 923 | 1012 | 1012 | 0.0026 | 16.0 | 672337 |
| logged-in-reader | desktop | /vi/reading/session/44aaaa68-83c2-4214-ad29-73921f15bc90 | 38 | critical | 0 | 0 | 0 | 0 | 3260 | 953 | 1134 | 1284 | 1284 | 0.0036 | 11.0 | 672113 |
| logged-in-reader | desktop | /vi/reading/session/05c3c239-ad35-4bc6-8521-0393949285ec | 38 | critical | 0 | 0 | 0 | 0 | 6789 | 2427 | 3653 | 1220 | 5100 | 0.0026 | 46.0 | 672027 |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 33 | high | 0 | 0 | 0 | 0 | 3993 | 1413 | 1413 | 1564 | 2168 | 0.0000 | 469.0 | 578836 |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 0 | 0 | 0 | 0 | 3863 | 1303 | 1351 | 1596 | 1596 | 0.0026 | 10.0 | 578698 |
| logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 34 | high | 0 | 0 | 1 | 0 | 2975 | 821 | 838 | 1092 | 1092 | 0.0026 | 41.0 | 579902 |
| logged-in-admin | mobile | /vi/admin | 35 | high | 0 | 0 | 0 | 0 | 3102 | 868 | 938 | 924 | 924 | 0.0000 | 44.0 | 597412 |
| logged-in-admin | mobile | /vi/wallet/deposit/history | 33 | high | 0 | 0 | 0 | 0 | 2934 | 892 | 892 | 976 | 976 | 0.0000 | 6.0 | 580823 |
| logged-in-admin | mobile | /vi/community | 43 | critical | 0 | 0 | 1 | 1 | 3917 | 811 | 1023 | 920 | 1720 | 0.0000 | 17.0 | 736222 |
| logged-in-admin | mobile | /vi/collection | 53 | critical | 0 | 0 | 1 | 0 | 4063 | 960 | 2006 | 796 | 1252 | 0.0000 | 252.0 | 4096102 |
| logged-in-admin | mobile | /vi/reading/session/9aad5329-8deb-4742-b41c-f4014714522c | 37 | critical | 0 | 0 | 0 | 0 | 2911 | 790 | 852 | 796 | 796 | 0.0000 | 1.0 | 641077 |
| logged-in-admin | mobile | /vi/reading/session/74178d4e-e08f-4d78-8a33-f95d0ab5f9f0 | 37 | critical | 0 | 0 | 0 | 0 | 2923 | 818 | 888 | 852 | 852 | 0.0000 | 7.0 | 641197 |
| logged-in-admin | mobile | /vi/reading/session/da39a32d-4f53-4149-98f1-4497f272a9ec | 37 | critical | 0 | 0 | 0 | 0 | 2973 | 873 | 908 | 852 | 852 | 0.0000 | 10.0 | 640965 |
| logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 33 | high | 0 | 0 | 0 | 0 | 3308 | 1068 | 1079 | 936 | 936 | 0.0000 | 10.0 | 579589 |
| logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 0 | 0 | 0 | 0 | 2957 | 816 | 855 | 868 | 868 | 0.0000 | 0.0 | 579461 |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 33 | high | 0 | 0 | 0 | 0 | 3024 | 921 | 964 | 980 | 980 | 0.0000 | 3.0 | 579782 |
| logged-in-reader | mobile | /vi/wallet/deposit/history | 34 | high | 0 | 0 | 1 | 0 | 3134 | 981 | 1019 | 872 | 1236 | 0.0000 | 1.0 | 581193 |
| logged-in-reader | mobile | /vi/community | 43 | critical | 0 | 0 | 1 | 1 | 4154 | 992 | 1220 | 1028 | 2060 | 0.0051 | 51.0 | 736117 |
| logged-in-reader | mobile | /vi/collection | 53 | critical | 0 | 0 | 1 | 0 | 4506 | 932 | 2359 | 792 | 2408 | 0.0000 | 643.0 | 4094653 |
| logged-in-reader | mobile | /vi/reading/session/2b9d5b67-ad07-4ce5-a341-06d9967d8e23 | 37 | critical | 0 | 0 | 0 | 0 | 2991 | 833 | 964 | 832 | 832 | 0.0000 | 9.0 | 641050 |
| logged-in-reader | mobile | /vi/reading/session/545ce85b-9d06-4e20-8fcd-6b4d48e4279d | 37 | critical | 0 | 0 | 0 | 0 | 2933 | 769 | 910 | 816 | 816 | 0.0000 | 0.0 | 640865 |
| logged-in-reader | mobile | /vi/reading/session/eb60f120-e06f-4a3a-bc97-93336d8ddb46 | 37 | critical | 0 | 0 | 0 | 0 | 2913 | 792 | 877 | 836 | 836 | 0.0000 | 0.0 | 640977 |
| logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 34 | high | 0 | 0 | 1 | 0 | 2897 | 871 | 871 | 812 | 1208 | 0.0000 | 5.0 | 580578 |
| logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 0 | 0 | 0 | 0 | 2899 | 838 | 882 | 900 | 900 | 0.0000 | 0.0 | 579587 |
| logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 33 | high | 0 | 0 | 0 | 0 | 2958 | 877 | 910 | 884 | 884 | 0.0000 | 2.0 | 579520 |

## Suspicious Pages (>25 requests)
| Scenario | Viewport | Route | Request Count | Severity | API | Static | Third-party |
| --- | --- | --- | ---: | --- | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/admin | 35 | high | 0 | 32 | 0 |
| logged-in-admin | desktop | /vi/wallet/deposit/history | 33 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | /vi/community | 44 | critical | 4 | 37 | 0 |
| logged-in-admin | desktop | /vi/collection | 105 | critical | 1 | 31 | 70 |
| logged-in-admin | desktop | /vi/reading/session/06513c39-08d1-416a-82af-e258832c0a5d | 38 | critical | 1 | 34 | 0 |
| logged-in-admin | desktop | /vi/reading/session/8a78f474-c140-476e-aba3-201e40196e9c | 38 | critical | 1 | 34 | 0 |
| logged-in-admin | desktop | /vi/reading/session/3f88391a-1ef5-4e45-9ab6-1d42268ad9ca | 38 | critical | 1 | 34 | 0 |
| logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 33 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 33 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi/wallet/deposit/history | 33 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi/community | 44 | critical | 4 | 37 | 0 |
| logged-in-reader | desktop | /vi/collection | 104 | critical | 0 | 31 | 70 |
| logged-in-reader | desktop | /vi/reading/session/e733f944-ccf4-4950-9ec5-a78e80cdc2e7 | 38 | critical | 1 | 34 | 0 |
| logged-in-reader | desktop | /vi/reading/session/44aaaa68-83c2-4214-ad29-73921f15bc90 | 38 | critical | 1 | 34 | 0 |
| logged-in-reader | desktop | /vi/reading/session/05c3c239-ad35-4bc6-8521-0393949285ec | 38 | critical | 1 | 34 | 0 |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 33 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 34 | high | 1 | 30 | 0 |
| logged-in-admin | mobile | /vi/admin | 35 | high | 0 | 32 | 0 |
| logged-in-admin | mobile | /vi/wallet/deposit/history | 33 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | /vi/community | 43 | critical | 3 | 37 | 0 |
| logged-in-admin | mobile | /vi/collection | 53 | critical | 1 | 31 | 18 |
| logged-in-admin | mobile | /vi/reading/session/9aad5329-8deb-4742-b41c-f4014714522c | 37 | critical | 1 | 33 | 0 |
| logged-in-admin | mobile | /vi/reading/session/74178d4e-e08f-4d78-8a33-f95d0ab5f9f0 | 37 | critical | 1 | 33 | 0 |
| logged-in-admin | mobile | /vi/reading/session/da39a32d-4f53-4149-98f1-4497f272a9ec | 37 | critical | 1 | 33 | 0 |
| logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 33 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 33 | high | 0 | 30 | 0 |
| logged-in-reader | mobile | /vi/wallet/deposit/history | 34 | high | 1 | 30 | 0 |
| logged-in-reader | mobile | /vi/community | 43 | critical | 3 | 37 | 0 |
| logged-in-reader | mobile | /vi/collection | 53 | critical | 1 | 31 | 18 |
| logged-in-reader | mobile | /vi/reading/session/2b9d5b67-ad07-4ce5-a341-06d9967d8e23 | 37 | critical | 1 | 33 | 0 |
| logged-in-reader | mobile | /vi/reading/session/545ce85b-9d06-4e20-8fcd-6b4d48e4279d | 37 | critical | 1 | 33 | 0 |
| logged-in-reader | mobile | /vi/reading/session/eb60f120-e06f-4a3a-bc97-93336d8ddb46 | 37 | critical | 1 | 33 | 0 |
| logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 34 | high | 1 | 30 | 0 |
| logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 33 | high | 0 | 30 | 0 |
| logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 33 | high | 0 | 30 | 0 |

## High Slow Requests (>800ms)
| Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 4114 | 132 | third-party | https://img.tarotnow.xyz/light-god-50/42_Six_of_Pentacles_50_20260325_181413.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 4096 | 90 | third-party | https://img.tarotnow.xyz/light-god-50/41_Five_of_Pentacles_50_20260325_181413.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 4017 | 85 | third-party | https://img.tarotnow.xyz/light-god-50/40_Four_of_Pentacles_50_20260325_181413.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3948 | 121 | third-party | https://img.tarotnow.xyz/light-god-50/39_Three_of_Pentacles_50_20260325_181411.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3837 | 139 | third-party | https://img.tarotnow.xyz/light-god-50/38_Two_of_Pentacles_50_20260325_181411.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3805 | 104 | third-party | https://img.tarotnow.xyz/light-god-50/37_Ace_of_Pentacles_50_20260325_181411.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3791 | 83 | third-party | https://img.tarotnow.xyz/light-god-50/64_King_of_Swords_50_20260325_181428.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3724 | 108 | third-party | https://img.tarotnow.xyz/light-god-50/63_Queen_of_Swords_50_20260325_181427.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3679 | 96 | third-party | https://img.tarotnow.xyz/light-god-50/62_Knight_of_Swords_50_20260325_181427.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3635 | 114 | third-party | https://img.tarotnow.xyz/light-god-50/61_Page_of_Swords_50_20260325_181427.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3622 | 130 | third-party | https://img.tarotnow.xyz/light-god-50/60_Ten_of_Swords_50_20260325_181426.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3575 | 95 | third-party | https://img.tarotnow.xyz/light-god-50/59_Nine_of_Swords_50_20260325_181426.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3567 | 95 | third-party | https://img.tarotnow.xyz/light-god-50/58_Eight_of_Swords_50_20260325_181426.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3547 | 84 | third-party | https://img.tarotnow.xyz/light-god-50/57_Seven_of_Swords_50_20260325_181424.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3537 | 93 | third-party | https://img.tarotnow.xyz/light-god-50/56_Six_of_Swords_50_20260325_181424.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3492 | 93 | third-party | https://img.tarotnow.xyz/light-god-50/55_Five_of_Swords_50_20260325_181424.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3481 | 88 | third-party | https://img.tarotnow.xyz/light-god-50/54_Four_of_Swords_50_20260325_181422.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3402 | 159 | third-party | https://img.tarotnow.xyz/light-god-50/53_Three_of_Swords_50_20260325_181422.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3388 | 202 | third-party | https://img.tarotnow.xyz/light-god-50/52_Two_of_Swords_50_20260325_181422.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3372 | 171 | third-party | https://img.tarotnow.xyz/light-god-50/51_Ace_of_Swords_50_20260325_181419.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3294 | 168 | third-party | https://img.tarotnow.xyz/light-god-50/36_King_of_Cups_50_20260325_181408.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3264 | 162 | third-party | https://img.tarotnow.xyz/light-god-50/35_Queen_of_Cups_50_20260325_181408.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3216 | 181 | third-party | https://img.tarotnow.xyz/light-god-50/34_Knight_of_Cups_50_20260325_181408.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3133 | 192 | third-party | https://img.tarotnow.xyz/light-god-50/33_Page_of_Cups_50_20260325_181406.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3091 | 211 | third-party | https://img.tarotnow.xyz/light-god-50/32_Ten_of_Cups_50_20260325_181406.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3028 | 170 | third-party | https://img.tarotnow.xyz/light-god-50/31_Nine_of_Cups_50_20260325_181406.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2960 | 173 | third-party | https://img.tarotnow.xyz/light-god-50/30_Eight_of_Cups_50_20260325_181405.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2952 | 177 | third-party | https://img.tarotnow.xyz/light-god-50/29_Seven_of_Cups_50_20260325_181405.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2902 | 177 | third-party | https://img.tarotnow.xyz/light-god-50/28_Six_of_Cups_50_20260325_181405.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2846 | 163 | third-party | https://img.tarotnow.xyz/light-god-50/27_Five_of_Cups_50_20260325_181402.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2824 | 178 | third-party | https://img.tarotnow.xyz/light-god-50/26_Four_of_Cups_50_20260325_181402.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2801 | 182 | third-party | https://img.tarotnow.xyz/light-god-50/25_Three_of_Cups_50_20260325_181402.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2786 | 134 | third-party | https://img.tarotnow.xyz/light-god-50/24_Two_of_Cups_50_20260325_181401.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2653 | 149 | third-party | https://img.tarotnow.xyz/light-god-50/23_Ace_of_Cups_50_20260325_181401.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2637 | 208 | third-party | https://img.tarotnow.xyz/light-god-50/78_King_of_Wands_50_20260325_181436.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 2586 | 269 | third-party | https://img.tarotnow.xyz/light-god-50/53_Three_of_Swords_50_20260325_181422.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 2577 | 267 | third-party | https://img.tarotnow.xyz/light-god-50/52_Two_of_Swords_50_20260325_181422.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 2547 | 295 | third-party | https://img.tarotnow.xyz/light-god-50/51_Ace_of_Swords_50_20260325_181419.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2510 | 100 | third-party | https://img.tarotnow.xyz/light-god-50/77_Queen_of_Wands_50_20260325_181436.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 2481 | 284 | third-party | https://img.tarotnow.xyz/light-god-50/36_King_of_Cups_50_20260325_181408.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 2443 | 282 | third-party | https://img.tarotnow.xyz/light-god-50/35_Queen_of_Cups_50_20260325_181408.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2435 | 128 | third-party | https://img.tarotnow.xyz/light-god-50/76_Knight_of_Wands_50_20260325_181436.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 2403 | 265 | third-party | https://img.tarotnow.xyz/light-god-50/34_Knight_of_Cups_50_20260325_181408.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2362 | 115 | third-party | https://img.tarotnow.xyz/light-god-50/75_Page_of_Wands_50_20260325_181435.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2322 | 103 | third-party | https://img.tarotnow.xyz/light-god-50/74_Ten_of_Wands_50_20260325_181435.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 2314 | 260 | third-party | https://img.tarotnow.xyz/light-god-50/33_Page_of_Cups_50_20260325_181406.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 2286 | 210 | third-party | https://img.tarotnow.xyz/light-god-50/32_Ten_of_Cups_50_20260325_181406.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2244 | 112 | third-party | https://img.tarotnow.xyz/light-god-50/73_Nine_of_Wands_50_20260325_181435.avif |
| logged-in-reader | desktop | /vi/reading/session/05c3c239-ad35-4bc6-8521-0393949285ec | GET | 200 | 2207 | 325 | html | https://www.tarotnow.xyz/vi/reading/session/05c3c239-ad35-4bc6-8521-0393949285ec |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2190 | 118 | third-party | https://img.tarotnow.xyz/light-god-50/72_Eight_of_Wands_50_20260325_181433.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 2169 | 182 | third-party | https://img.tarotnow.xyz/light-god-50/31_Nine_of_Cups_50_20260325_181406.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2157 | 148 | third-party | https://img.tarotnow.xyz/light-god-50/71_Seven_of_Wands_50_20260325_181433.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2150 | 136 | third-party | https://img.tarotnow.xyz/light-god-50/70_Six_of_Wands_50_20260325_181433.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2110 | 138 | third-party | https://img.tarotnow.xyz/light-god-50/69_Five_of%20_Wands_50_20260325_181431.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 2100 | 266 | third-party | https://img.tarotnow.xyz/light-god-50/30_Eight_of_Cups_50_20260325_181405.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 2077 | 286 | third-party | https://img.tarotnow.xyz/light-god-50/29_Seven_of_Cups_50_20260325_181405.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 2076 | 292 | third-party | https://img.tarotnow.xyz/light-god-50/28_Six_of_Cups_50_20260325_181405.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2054 | 145 | third-party | https://img.tarotnow.xyz/light-god-50/68_Four_of_Wands_50_20260325_181431.avif |
| logged-in-reader | desktop | /vi/reading/session/05c3c239-ad35-4bc6-8521-0393949285ec | GET | 200 | 2000 | 163 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 1998 | 132 | third-party | https://img.tarotnow.xyz/light-god-50/67_Three_of_Wands_50_20260325_181431.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 1977 | 151 | third-party | https://img.tarotnow.xyz/light-god-50/26_Four_of_Cups_50_20260325_181402.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 1974 | 153 | third-party | https://img.tarotnow.xyz/light-god-50/27_Five_of_Cups_50_20260325_181402.avif |
| logged-in-reader | desktop | /vi/reading/session/05c3c239-ad35-4bc6-8521-0393949285ec | GET | 200 | 1965 | 170 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 1959 | 143 | third-party | https://img.tarotnow.xyz/light-god-50/66_Two_of_Wands_50_20260325_181428.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 1942 | 121 | third-party | https://img.tarotnow.xyz/light-god-50/25_Three_of_Cups_50_20260325_181402.avif |
| logged-in-reader | desktop | /vi/reading/session/05c3c239-ad35-4bc6-8521-0393949285ec | GET | 200 | 1904 | 304 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | desktop | /vi/reading/session/05c3c239-ad35-4bc6-8521-0393949285ec | GET | 200 | 1892 | 133 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 1884 | 143 | third-party | https://img.tarotnow.xyz/light-god-50/65_Ace_of_Wands_50_20260325_181428.avif |
| logged-in-reader | desktop | /vi/reading/session/05c3c239-ad35-4bc6-8521-0393949285ec | GET | 200 | 1881 | 119 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg6ntv_3jdd4.js |
| logged-in-reader | desktop | /vi/reading/session/05c3c239-ad35-4bc6-8521-0393949285ec | GET | 200 | 1840 | 84 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-0-hlnlst9psev.js |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 1838 | 126 | third-party | https://img.tarotnow.xyz/light-god-50/24_Two_of_Cups_50_20260325_181401.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 1835 | 95 | third-party | https://img.tarotnow.xyz/light-god-50/22_The_World_50_20260325_181401.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 1747 | 146 | third-party | https://img.tarotnow.xyz/light-god-50/23_Ace_of_Cups_50_20260325_181401.avif |
| logged-in-reader | desktop | /vi/reading/session/05c3c239-ad35-4bc6-8521-0393949285ec | GET | 200 | 1741 | 94 | static | https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-reader | desktop | /vi/reading/session/05c3c239-ad35-4bc6-8521-0393949285ec | GET | 200 | 1740 | 89 | static | https://www.tarotnow.xyz/_next/static/chunks/0fqgq6m2b-440.css |
| logged-in-reader | desktop | /vi/reading/session/05c3c239-ad35-4bc6-8521-0393949285ec | GET | 200 | 1739 | 80 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-reader | desktop | /vi/reading/session/05c3c239-ad35-4bc6-8521-0393949285ec | GET | 200 | 1727 | 117 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-reader | desktop | /vi/reading/session/05c3c239-ad35-4bc6-8521-0393949285ec | GET | 200 | 1721 | 117 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 1720 | 95 | third-party | https://img.tarotnow.xyz/light-god-50/21_Judgement_50_20260325_181359.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 1713 | 186 | third-party | https://img.tarotnow.xyz/light-god-50/78_King_of_Wands_50_20260325_181436.avif |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-reader | desktop | /vi/community | GET | 200 | 797 | 303 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 796 | 64 | static | https://www.tarotnow.xyz/_next/static/chunks/14zfgnebl8n68.js |
| logged-in-admin | mobile | /vi/collection | GET | 200 | 794 | 183 | third-party | https://img.tarotnow.xyz/light-god-50/11_Wheel_of%20_Fortune_50_20260325_181353.avif |
| logged-in-admin | mobile | /vi/collection | GET | 200 | 789 | 192 | third-party | https://img.tarotnow.xyz/light-god-50/08_The_Chariot_50_20260325_181351.avif |
| logged-in-reader | mobile | /vi/collection | GET | 200 | 787 | 123 | third-party | https://img.tarotnow.xyz/light-god-50/01_The_Fool_50_20260325_181348.avif |
| logged-in-admin | desktop | /vi/admin | GET | 200 | 786 | 413 | html | https://www.tarotnow.xyz/vi/admin |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 784 | 129 | third-party | https://img.tarotnow.xyz/light-god-50/14_Death_50_20260325_181356.avif |
| logged-in-admin | desktop | /vi/wallet/deposit/history | GET | 200 | 782 | 307 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | desktop | /vi/community | GET | 200 | 779 | 325 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 778 | 111 | third-party | https://img.tarotnow.xyz/light-god-50/13_The_Hanged_Man_50_20260325_181356.avif |
| logged-in-admin | mobile | /vi/collection | GET | 200 | 774 | 189 | third-party | https://img.tarotnow.xyz/light-god-50/09_Strength_50_20260325_181351.avif |
| logged-in-admin | mobile | /vi/collection | GET | 200 | 768 | 167 | third-party | https://img.tarotnow.xyz/light-god-50/10_The_Hermit_50_20260325_181353.avif |
| logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 764 | 314 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 761 | 200 | third-party | https://img.tarotnow.xyz/light-god-50/02_The_Magician_50_20260325_181348.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 757 | 434 | third-party | https://img.tarotnow.xyz/light-god-50/06_The_Hierophant_50_20260325_181348.avif |
| logged-in-admin | desktop | /vi/community | GET | 200 | 756 | 291 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 750 | 437 | third-party | https://img.tarotnow.xyz/light-god-50/03_The_High%20Priestess%20_50_20260325_181348.avif |
| logged-in-admin | mobile | /vi/collection | GET | 200 | 740 | 97 | third-party | https://img.tarotnow.xyz/light-god-50/03_The_High%20Priestess%20_50_20260325_181348.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 737 | 112 | third-party | https://img.tarotnow.xyz/light-god-50/12_Justice_50_20260325_181353.avif |
| logged-in-admin | desktop | /vi/reading/session/3f88391a-1ef5-4e45-9ab6-1d42268ad9ca | GET | 200 | 726 | 370 | html | https://www.tarotnow.xyz/vi/reading/session/3f88391a-1ef5-4e45-9ab6-1d42268ad9ca |
| logged-in-admin | desktop | /vi/wallet/deposit/history | GET | 200 | 723 | 379 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | mobile | /vi/admin | GET | 200 | 713 | 306 | html | https://www.tarotnow.xyz/vi/admin |
| logged-in-reader | desktop | /vi/wallet/deposit/history | GET | 200 | 710 | 304 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 710 | 434 | third-party | https://img.tarotnow.xyz/light-god-50/05_The_Emperor_50_20260325_181348.avif |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 704 | 192 | static | https://www.tarotnow.xyz/_next/static/chunks/167n5~xbg2bxe.js |
| logged-in-reader | desktop | /vi/community | GET | 200 | 703 | 322 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 701 | 113 | third-party | https://img.tarotnow.xyz/light-god-50/11_Wheel_of%20_Fortune_50_20260325_181353.avif |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 695 | 400 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-reader | mobile | /vi/reading/session/2b9d5b67-ad07-4ce5-a341-06d9967d8e23 | GET | 200 | 694 | 331 | html | https://www.tarotnow.xyz/vi/reading/session/2b9d5b67-ad07-4ce5-a341-06d9967d8e23 |
| logged-in-admin | mobile | /vi/reading/session/74178d4e-e08f-4d78-8a33-f95d0ab5f9f0 | GET | 200 | 687 | 326 | html | https://www.tarotnow.xyz/vi/reading/session/74178d4e-e08f-4d78-8a33-f95d0ab5f9f0 |
| logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 685 | 295 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | mobile | /vi/community | GET | 200 | 684 | 339 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 684 | 408 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-admin | mobile | /vi/collection | GET | 200 | 681 | 397 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=96&q=75 |
| logged-in-admin | mobile | /vi/reading/session/da39a32d-4f53-4149-98f1-4497f272a9ec | GET | 200 | 681 | 301 | api | https://www.tarotnow.xyz/api/reading/cards-catalog |
| logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 676 | 372 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 672 | 164 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-admin | mobile | /vi/collection | GET | 200 | 667 | 97 | third-party | https://img.tarotnow.xyz/light-god-50/05_The_Emperor_50_20260325_181348.avif |
| logged-in-admin | desktop | /vi/wallet/deposit/history | GET | 200 | 659 | 81 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 656 | 323 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-reader | desktop | /vi/reading/session/e733f944-ccf4-4950-9ec5-a78e80cdc2e7 | GET | 200 | 655 | 319 | html | https://www.tarotnow.xyz/vi/reading/session/e733f944-ccf4-4950-9ec5-a78e80cdc2e7 |
| logged-in-reader | mobile | /vi/community | GET | 404 | 655 | 640 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F1bf7374304584c0488e06621bbc1454f.webp&w=128&q=75 |
| logged-in-admin | mobile | /vi/reading/session/da39a32d-4f53-4149-98f1-4497f272a9ec | GET | 200 | 654 | 307 | html | https://www.tarotnow.xyz/vi/reading/session/da39a32d-4f53-4149-98f1-4497f272a9ec |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 653 | 300 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | mobile | /vi/wallet/deposit/history | GET | 200 | 653 | 436 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | desktop | /vi/reading/session/8a78f474-c140-476e-aba3-201e40196e9c | GET | 200 | 652 | 301 | html | https://www.tarotnow.xyz/vi/reading/session/8a78f474-c140-476e-aba3-201e40196e9c |
| logged-in-reader | desktop | /vi/wallet/deposit/history | GET | 200 | 652 | 78 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 652 | 361 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-reader | mobile | /vi/reading/session/eb60f120-e06f-4a3a-bc97-93336d8ddb46 | GET | 200 | 651 | 326 | html | https://www.tarotnow.xyz/vi/reading/session/eb60f120-e06f-4a3a-bc97-93336d8ddb46 |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 650 | 373 | static | https://www.tarotnow.xyz/_next/static/chunks/14zfgnebl8n68.js |
| logged-in-reader | mobile | /vi/reading/session/2b9d5b67-ad07-4ce5-a341-06d9967d8e23 | GET | 200 | 650 | 309 | api | https://www.tarotnow.xyz/api/reading/cards-catalog |
| logged-in-reader | mobile | /vi/reading/session/545ce85b-9d06-4e20-8fcd-6b4d48e4279d | GET | 200 | 648 | 312 | html | https://www.tarotnow.xyz/vi/reading/session/545ce85b-9d06-4e20-8fcd-6b4d48e4279d |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 641 | 126 | static | https://www.tarotnow.xyz/_next/static/chunks/08q.cp_si1m5q.js |
| logged-in-reader | desktop | /vi/reading/session/44aaaa68-83c2-4214-ad29-73921f15bc90 | GET | 200 | 640 | 448 | html | https://www.tarotnow.xyz/vi/reading/session/44aaaa68-83c2-4214-ad29-73921f15bc90 |
| logged-in-admin | mobile | /vi/reading/session/9aad5329-8deb-4742-b41c-f4014714522c | GET | 200 | 640 | 329 | html | https://www.tarotnow.xyz/vi/reading/session/9aad5329-8deb-4742-b41c-f4014714522c |
| logged-in-reader | desktop | /vi/wallet/deposit/history | GET | 200 | 637 | 97 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | desktop | /vi/admin | GET | 200 | 634 | 68 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | desktop | /vi/community | GET | 200 | 634 | 582 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F86c96d40a901461f991ebcc1acb4cf48.webp&w=32&q=75 |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 633 | 156 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-reader | desktop | /vi/community | GET | 200 | 627 | 76 | static | https://www.tarotnow.xyz/_next/static/chunks/14zfgnebl8n68.js |
| logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 625 | 420 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-admin | desktop | /vi/community | GET | 200 | 624 | 519 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=32&q=75 |
| logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 623 | 357 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 623 | 188 | static | https://www.tarotnow.xyz/_next/static/chunks/04wgpsc4kh~3w.js |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 622 | 189 | static | https://www.tarotnow.xyz/_next/static/chunks/08nnjyw~vjmez.js |
| logged-in-admin | mobile | /vi/community | GET | 200 | 621 | 315 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | desktop | /vi/reading/session/06513c39-08d1-416a-82af-e258832c0a5d | GET | 200 | 615 | 304 | html | https://www.tarotnow.xyz/vi/reading/session/06513c39-08d1-416a-82af-e258832c0a5d |
| logged-in-reader | desktop | /vi/community | GET | 200 | 612 | 57 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 610 | 353 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | mobile | /vi/reading/session/74178d4e-e08f-4d78-8a33-f95d0ab5f9f0 | GET | 200 | 609 | 307 | api | https://www.tarotnow.xyz/api/reading/cards-catalog |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 608 | 114 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | desktop | /vi/wallet/deposit/history | GET | 200 | 607 | 73 | static | https://www.tarotnow.xyz/_next/static/chunks/08q.cp_si1m5q.js |
| logged-in-reader | desktop | /vi/reading/session/44aaaa68-83c2-4214-ad29-73921f15bc90 | GET | 200 | 607 | 316 | api | https://www.tarotnow.xyz/api/reading/cards-catalog |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 602 | 120 | static | https://www.tarotnow.xyz/_next/static/chunks/07tk2ft0d9n3x.js |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 600 | 83 | static | https://www.tarotnow.xyz/_next/static/chunks/0r68tfefeu~dz.js |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 597 | 81 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | desktop | /vi/wallet/deposit/history | GET | 200 | 596 | 95 | static | https://www.tarotnow.xyz/_next/static/chunks/08q.cp_si1m5q.js |
| logged-in-admin | desktop | /vi/wallet/deposit/history | GET | 200 | 593 | 71 | static | https://www.tarotnow.xyz/_next/static/chunks/0hf3ddbnzc0yj.js |
| logged-in-admin | desktop | /vi/wallet/deposit/history | GET | 200 | 592 | 70 | static | https://www.tarotnow.xyz/_next/static/chunks/167n5~xbg2bxe.js |
| logged-in-admin | desktop | /vi/reading/session/06513c39-08d1-416a-82af-e258832c0a5d | GET | 200 | 590 | 318 | api | https://www.tarotnow.xyz/api/reading/cards-catalog |

## Pending Requests
| Scenario | Viewport | Route | URL |
| --- | --- | --- | --- |
| logged-in-reader | desktop | /vi/collection | https://img.tarotnow.xyz/light-god-50/54_Four_of_Swords_50_20260325_181422.avif |
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
| logged-in-admin | desktop | reading.init.daily_1: blocked (400). |
| logged-in-admin | desktop | reading.init.spread_3: created 06513c39-08d1-416a-82af-e258832c0a5d. |
| logged-in-admin | desktop | reading.init.spread_5: created 8a78f474-c140-476e-aba3-201e40196e9c. |
| logged-in-admin | desktop | reading.init.spread_10: created 3f88391a-1ef5-4e45-9ab6-1d42268ad9ca. |
| logged-in-admin | desktop | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-admin | desktop | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-admin | desktop | reader-detail:ui-discovery-empty |
| logged-in-admin | desktop | chat-room-detail:ui-discovery-empty |
| logged-in-admin | desktop | reading-history-detail:ui-discovery-empty |
| logged-in-admin | desktop | community-posts:api-discovery-1 |
| logged-in-admin | desktop | community-post-detail:69db54fc297f66f734421a3c:200 |
| logged-in-reader | desktop | origin-discovery:/sitemap.xml:routes-13 |
| logged-in-reader | desktop | origin-discovery:/robots.txt:routes-13 |
| logged-in-reader | desktop | reading.init.daily_1: blocked (400). |
| logged-in-reader | desktop | reading.init.spread_3: created e733f944-ccf4-4950-9ec5-a78e80cdc2e7. |
| logged-in-reader | desktop | reading.init.spread_5: created 44aaaa68-83c2-4214-ad29-73921f15bc90. |
| logged-in-reader | desktop | reading.init.spread_10: created 05c3c239-ad35-4bc6-8521-0393949285ec. |
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
| logged-in-admin | mobile | reading.init.spread_3: created 9aad5329-8deb-4742-b41c-f4014714522c. |
| logged-in-admin | mobile | reading.init.spread_5: created 74178d4e-e08f-4d78-8a33-f95d0ab5f9f0. |
| logged-in-admin | mobile | reading.init.spread_10: created da39a32d-4f53-4149-98f1-4497f272a9ec. |
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
| logged-in-reader | mobile | reading.init.spread_3: created 2b9d5b67-ad07-4ce5-a341-06d9967d8e23. |
| logged-in-reader | mobile | reading.init.spread_5: created 545ce85b-9d06-4e20-8fcd-6b4d48e4279d. |
| logged-in-reader | mobile | reading.init.spread_10: created eb60f120-e06f-4a3a-bc97-93336d8ddb46. |
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
