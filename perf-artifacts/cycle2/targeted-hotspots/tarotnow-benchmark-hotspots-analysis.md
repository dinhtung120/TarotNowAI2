# TarotNow Benchmark Analysis

- Run time (UTC): 2026-04-29T21:00:33.240Z
- Base: https://www.tarotnow.xyz/vi
- Benchmark mode: targeted-hotspots
- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader

## Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 0 | 0.0 | 0 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 10 | 43.0 | 3632 | 0 | 0 | 0 | 3 | 1 | yes |
| logged-in-reader | desktop | 9 | 43.9 | 3945 | 17 | 0 | 0 | 3 | 1 | yes |
| logged-out | mobile | 0 | 0.0 | 0 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 10 | 37.4 | 3211 | 0 | 0 | 0 | 2 | 1 | yes |
| logged-in-reader | mobile | 9 | 37.9 | 3265 | 0 | 0 | 0 | 4 | 1 | yes |

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

## Key Findings
1. Critical: 20 page(s) vượt 35 requests.
2. High: 38 page(s) vượt 25 requests.
3. High: 189 request vượt 800ms.
4. Medium: 219 request trong dải 400-800ms.
5. Không phát hiện handshake redirect bất thường.

## Top Slow Pages
| Scenario | Viewport | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/collection | 7574 | 105 | 2652 | 347.0 | 0.0029 |
| logged-in-reader | desktop | /vi/reading/session/05c3c239-ad35-4bc6-8521-0393949285ec | 6789 | 38 | 5100 | 46.0 | 0.0026 |
| logged-in-reader | mobile | /vi/collection | 4506 | 53 | 2408 | 643.0 | 0.0000 |
| logged-in-reader | desktop | /vi/community | 4199 | 44 | 2144 | 43.0 | 0.0026 |
| logged-in-admin | desktop | /vi/community | 4194 | 44 | 1980 | 86.0 | 0.0029 |
| logged-in-reader | desktop | /vi/collection | 4185 | 104 | 2192 | 178.0 | 0.0026 |
| logged-in-reader | mobile | /vi/community | 4154 | 43 | 2060 | 51.0 | 0.0051 |
| logged-in-admin | mobile | /vi/collection | 4063 | 53 | 1252 | 252.0 | 0.0000 |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 3993 | 33 | 2168 | 469.0 | 0.0000 |
| logged-in-admin | mobile | /vi/community | 3917 | 43 | 1720 | 17.0 | 0.0000 |

## High Slow Requests (> 800ms)
| Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 4114 | 132 | https://img.tarotnow.xyz/light-god-50/42_Six_of_Pentacles_50_20260325_181413.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 4096 | 90 | https://img.tarotnow.xyz/light-god-50/41_Five_of_Pentacles_50_20260325_181413.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 4017 | 85 | https://img.tarotnow.xyz/light-god-50/40_Four_of_Pentacles_50_20260325_181413.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3948 | 121 | https://img.tarotnow.xyz/light-god-50/39_Three_of_Pentacles_50_20260325_181411.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3837 | 139 | https://img.tarotnow.xyz/light-god-50/38_Two_of_Pentacles_50_20260325_181411.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3805 | 104 | https://img.tarotnow.xyz/light-god-50/37_Ace_of_Pentacles_50_20260325_181411.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3791 | 83 | https://img.tarotnow.xyz/light-god-50/64_King_of_Swords_50_20260325_181428.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3724 | 108 | https://img.tarotnow.xyz/light-god-50/63_Queen_of_Swords_50_20260325_181427.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3679 | 96 | https://img.tarotnow.xyz/light-god-50/62_Knight_of_Swords_50_20260325_181427.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3635 | 114 | https://img.tarotnow.xyz/light-god-50/61_Page_of_Swords_50_20260325_181427.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3622 | 130 | https://img.tarotnow.xyz/light-god-50/60_Ten_of_Swords_50_20260325_181426.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3575 | 95 | https://img.tarotnow.xyz/light-god-50/59_Nine_of_Swords_50_20260325_181426.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3567 | 95 | https://img.tarotnow.xyz/light-god-50/58_Eight_of_Swords_50_20260325_181426.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3547 | 84 | https://img.tarotnow.xyz/light-god-50/57_Seven_of_Swords_50_20260325_181424.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 3537 | 93 | https://img.tarotnow.xyz/light-god-50/56_Six_of_Swords_50_20260325_181424.avif |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-reader | desktop | /vi/community | GET | 200 | 797 | 303 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 796 | 64 | https://www.tarotnow.xyz/_next/static/chunks/14zfgnebl8n68.js |
| logged-in-admin | mobile | /vi/collection | GET | 200 | 794 | 183 | https://img.tarotnow.xyz/light-god-50/11_Wheel_of%20_Fortune_50_20260325_181353.avif |
| logged-in-admin | mobile | /vi/collection | GET | 200 | 789 | 192 | https://img.tarotnow.xyz/light-god-50/08_The_Chariot_50_20260325_181351.avif |
| logged-in-reader | mobile | /vi/collection | GET | 200 | 787 | 123 | https://img.tarotnow.xyz/light-god-50/01_The_Fool_50_20260325_181348.avif |
| logged-in-admin | desktop | /vi/admin | GET | 200 | 786 | 413 | https://www.tarotnow.xyz/vi/admin |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 784 | 129 | https://img.tarotnow.xyz/light-god-50/14_Death_50_20260325_181356.avif |
| logged-in-admin | desktop | /vi/wallet/deposit/history | GET | 200 | 782 | 307 | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | desktop | /vi/community | GET | 200 | 779 | 325 | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 778 | 111 | https://img.tarotnow.xyz/light-god-50/13_The_Hanged_Man_50_20260325_181356.avif |
| logged-in-admin | mobile | /vi/collection | GET | 200 | 774 | 189 | https://img.tarotnow.xyz/light-god-50/09_Strength_50_20260325_181351.avif |
| logged-in-admin | mobile | /vi/collection | GET | 200 | 768 | 167 | https://img.tarotnow.xyz/light-god-50/10_The_Hermit_50_20260325_181353.avif |
| logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 764 | 314 | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 761 | 200 | https://img.tarotnow.xyz/light-god-50/02_The_Magician_50_20260325_181348.avif |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 757 | 434 | https://img.tarotnow.xyz/light-god-50/06_The_Hierophant_50_20260325_181348.avif |

## Duplicate Request Candidates (Non-telemetry)
| Scenario | Viewport | Route | Count | Request Key |
| --- | --- | --- | ---: | --- |
| - | - | - | - | - |

## Notes
- Duplicate `/cdn-cgi/rum` được xem là telemetry của Cloudflare, không coi là business over-fetch.
- Coverage dynamic route có thể bị `coverage-blocked` nếu environment không có dữ liệu phù hợp tại thời điểm chạy.