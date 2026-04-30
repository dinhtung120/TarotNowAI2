# TarotNow Navigation Benchmark Report

- Generated at (UTC): 2026-04-30T07:28:52.002Z
- Base URL: https://www.tarotnow.xyz/vi
- Benchmark mode: targeted-hotspots
- Thresholds: >35 requests = Critical, >25 = High, request >800ms = High, >400ms = Medium
- Critical pages (request count): 7
- High pages (request count): 38
- High slow requests: 4
- Medium slow requests: 70

## Scenario Summary
| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Document Reloads | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 0 | 0 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 10 | 3652 | 330 | 8 | 0 | 3 | 1 | yes |
| logged-in-reader | desktop | 9 | 3467 | 293 | 0 | 0 | 0 | 0 | yes |
| logged-out | mobile | 0 | 0 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 10 | 3211 | 319 | 0 | 0 | 0 | 0 | yes |
| logged-in-reader | mobile | 9 | 3233 | 291 | 0 | 0 | 0 | 0 | yes |

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

## Per-Page Metrics
| Scenario | Viewport | Route | Requests | Interaction Requests | Severity | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Collection Img Requests | Collection Img 400-800ms | Collection Img >800ms | Collection Img First Load | Collection Img Reopen | Collection Img Cache Hits | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |
| --- | --- | --- | ---: | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/admin | 33 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2687 | 660 | 676 | 608 | 984 | 0.0000 | 0.0 | 596316 |
| logged-in-admin | desktop | /vi/wallet/deposit/history | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 2841 | 817 | 830 | 632 | 1196 | 0.0029 | 0.0 | 578906 |
| logged-in-admin | desktop | /vi/community | 41 | 3 | critical | 0 | 0 | 2 | 1 | 3 | 3 | 0 | 3 | 0 | 0 | 6265 | 3406 | 3508 | 3116 | 4076 | 0.0029 | 0.0 | 726416 |
| logged-in-admin | desktop | /vi/collection | 32 | 22 | high | 0 | 0 | 0 | 0 | 17 | 3 | 1 | 17 | 0 | 0 | 3819 | 807 | 823 | 748 | 748 | 0.0031 | 0.0 | 589107 |
| logged-in-admin | desktop | /vi/reading/session/a193d0c6-9a62-42a1-8135-7631ad6d078f | 38 | 1 | critical | 0 | 0 | 1 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 7007 | 4880 | 4986 | 4064 | 4932 | 0.0029 | 0.0 | 672557 |
| logged-in-admin | desktop | /vi/reading/session/367dda95-4b5a-48f9-8928-412c5a307556 | 31 | 8 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2761 | 712 | 725 | 632 | 1016 | 0.0029 | 0.0 | 578604 |
| logged-in-admin | desktop | /vi/reading/session/9e4d5054-f195-4546-b72a-e6d3b37b7939 | 31 | 7 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2967 | 929 | 938 | 748 | 1140 | 0.0029 | 0.0 | 578442 |
| logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2751 | 730 | 742 | 688 | 1088 | 0.0029 | 0.0 | 577468 |
| logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 2747 | 724 | 736 | 704 | 1080 | 0.0029 | 0.0 | 577729 |
| logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2678 | 654 | 668 | 580 | 968 | 0.0029 | 0.0 | 577677 |
| logged-in-reader | desktop | /vi/wallet/deposit/history | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2874 | 830 | 849 | 796 | 1252 | 0.0026 | 7.0 | 578523 |
| logged-in-reader | desktop | /vi/community | 32 | 12 | high | 0 | 0 | 0 | 0 | 3 | 2 | 0 | 3 | 0 | 0 | 3518 | 754 | 765 | 588 | 1792 | 0.0026 | 0.0 | 588943 |
| logged-in-reader | desktop | /vi/collection | 32 | 26 | high | 0 | 0 | 0 | 0 | 12 | 3 | 0 | 12 | 0 | 0 | 6927 | 780 | 804 | 624 | 624 | 0.0026 | 13.0 | 588293 |
| logged-in-reader | desktop | /vi/reading/session/59c2690a-55fc-42b5-9b56-b672ee0f49e5 | 37 | 1 | critical | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3015 | 829 | 983 | 572 | 948 | 0.0026 | 0.0 | 672425 |
| logged-in-reader | desktop | /vi/reading/session/88370553-4dca-4efb-ad03-037829d0e62b | 31 | 8 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2794 | 745 | 757 | 736 | 1172 | 0.0026 | 0.0 | 578778 |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | 37 | 1 | critical | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3630 | 1155 | 1603 | 588 | 3596 | 0.0000 | 0.0 | 672162 |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2834 | 786 | 802 | 640 | 1024 | 0.0026 | 0.0 | 577697 |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2821 | 761 | 765 | 604 | 984 | 0.0026 | 0.0 | 577837 |
| logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2793 | 758 | 770 | 604 | 1016 | 0.0026 | 0.0 | 577801 |
| logged-in-admin | mobile | /vi/admin | 33 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2780 | 728 | 742 | 572 | 896 | 0.0000 | 0.0 | 596150 |
| logged-in-admin | mobile | /vi/wallet/deposit/history | 31 | 2 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2882 | 836 | 858 | 560 | 920 | 0.0000 | 0.0 | 578762 |
| logged-in-admin | mobile | /vi/community | 32 | 12 | high | 0 | 0 | 0 | 0 | 3 | 2 | 0 | 3 | 0 | 0 | 3490 | 713 | 735 | 564 | 1716 | 0.0051 | 0.0 | 588890 |
| logged-in-admin | mobile | /vi/collection | 32 | 33 | high | 0 | 0 | 0 | 0 | 23 | 13 | 0 | 22 | 1 | 0 | 6319 | 659 | 737 | 492 | 492 | 0.0000 | 12.0 | 588860 |
| logged-in-admin | mobile | /vi/reading/session/47bd41c1-1413-4d9e-ab1f-af581c197f93 | 31 | 7 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2746 | 664 | 683 | 472 | 812 | 0.0000 | 0.0 | 578600 |
| logged-in-admin | mobile | /vi/reading/session/69338159-5cf0-40dc-ac0d-f3a699b43a23 | 31 | 7 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2769 | 712 | 728 | 476 | 812 | 0.0000 | 0.0 | 578796 |
| logged-in-admin | mobile | /vi/reading/session/b4d47003-af58-4b12-b2ae-381a7db36117 | 36 | 1 | critical | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2932 | 801 | 905 | 456 | 796 | 0.0000 | 0.0 | 641032 |
| logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 31 | 2 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2801 | 776 | 786 | 564 | 888 | 0.0000 | 0.0 | 577433 |
| logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2689 | 659 | 672 | 472 | 808 | 0.0000 | 0.0 | 577546 |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2701 | 672 | 689 | 448 | 772 | 0.0000 | 0.0 | 577461 |
| logged-in-reader | mobile | /vi/wallet/deposit/history | 31 | 2 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2871 | 794 | 809 | 668 | 668 | 0.0000 | 0.0 | 578315 |
| logged-in-reader | mobile | /vi/community | 32 | 12 | high | 0 | 0 | 0 | 0 | 3 | 2 | 0 | 3 | 0 | 0 | 3442 | 679 | 698 | 480 | 1644 | 0.0051 | 0.0 | 589038 |
| logged-in-reader | mobile | /vi/collection | 32 | 26 | high | 0 | 0 | 0 | 0 | 12 | 3 | 0 | 12 | 0 | 0 | 5601 | 734 | 756 | 512 | 512 | 0.0000 | 9.0 | 588298 |
| logged-in-reader | mobile | /vi/reading/session/b48c44dc-9497-49af-8129-a32dd71f65ff | 31 | 7 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2830 | 795 | 816 | 612 | 952 | 0.0000 | 0.0 | 578852 |
| logged-in-reader | mobile | /vi/reading/session/b3918917-e26b-429d-b4ee-51dedf38e2cb | 36 | 1 | critical | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2974 | 806 | 943 | 488 | 844 | 0.0000 | 0.0 | 640960 |
| logged-in-reader | mobile | /vi/reading/session/a9cf7586-252a-432a-bfc1-c0ecee4aac30 | 36 | 1 | critical | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2959 | 756 | 939 | 488 | 836 | 0.0000 | 0.0 | 640949 |
| logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2717 | 675 | 698 | 460 | 784 | 0.0000 | 0.0 | 577907 |
| logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 31 | 3 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2746 | 650 | 664 | 452 | 768 | 0.0000 | 0.0 | 577961 |
| logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 31 | 2 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2955 | 799 | 818 | 488 | 828 | 0.0000 | 0.0 | 577797 |

## Collection Image Focus
| Scenario | Viewport | Route | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/collection | 17 | 3 | 1 | 17 | 0 | 0 |
| logged-in-reader | desktop | /vi/collection | 12 | 3 | 0 | 12 | 0 | 0 |
| logged-in-admin | mobile | /vi/collection | 23 | 13 | 0 | 22 | 1 | 0 |
| logged-in-reader | mobile | /vi/collection | 12 | 3 | 0 | 12 | 0 | 0 |

## Suspicious Pages (>25 requests)
| Scenario | Viewport | Route | Request Count | Severity | API | Static | Third-party |
| --- | --- | --- | ---: | --- | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/admin | 33 | high | 0 | 31 | 0 |
| logged-in-admin | desktop | /vi/wallet/deposit/history | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | /vi/community | 41 | critical | 3 | 36 | 0 |
| logged-in-admin | desktop | /vi/collection | 32 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | /vi/reading/session/a193d0c6-9a62-42a1-8135-7631ad6d078f | 38 | critical | 2 | 34 | 0 |
| logged-in-admin | desktop | /vi/reading/session/367dda95-4b5a-48f9-8928-412c5a307556 | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | /vi/reading/session/9e4d5054-f195-4546-b72a-e6d3b37b7939 | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 31 | high | 0 | 29 | 0 |
| logged-in-reader | desktop | /vi/wallet/deposit/history | 31 | high | 0 | 29 | 0 |
| logged-in-reader | desktop | /vi/community | 32 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi/collection | 32 | high | 0 | 30 | 0 |
| logged-in-reader | desktop | /vi/reading/session/59c2690a-55fc-42b5-9b56-b672ee0f49e5 | 37 | critical | 1 | 34 | 0 |
| logged-in-reader | desktop | /vi/reading/session/88370553-4dca-4efb-ad03-037829d0e62b | 31 | high | 0 | 29 | 0 |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | 37 | critical | 1 | 34 | 0 |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | 31 | high | 0 | 29 | 0 |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 31 | high | 0 | 29 | 0 |
| logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | 31 | high | 0 | 29 | 0 |
| logged-in-admin | mobile | /vi/admin | 33 | high | 0 | 31 | 0 |
| logged-in-admin | mobile | /vi/wallet/deposit/history | 31 | high | 0 | 29 | 0 |
| logged-in-admin | mobile | /vi/community | 32 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | /vi/collection | 32 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | /vi/reading/session/47bd41c1-1413-4d9e-ab1f-af581c197f93 | 31 | high | 0 | 29 | 0 |
| logged-in-admin | mobile | /vi/reading/session/69338159-5cf0-40dc-ac0d-f3a699b43a23 | 31 | high | 0 | 29 | 0 |
| logged-in-admin | mobile | /vi/reading/session/b4d47003-af58-4b12-b2ae-381a7db36117 | 36 | critical | 1 | 33 | 0 |
| logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | 31 | high | 0 | 29 | 0 |
| logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 31 | high | 0 | 29 | 0 |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 31 | high | 0 | 29 | 0 |
| logged-in-reader | mobile | /vi/wallet/deposit/history | 31 | high | 0 | 29 | 0 |
| logged-in-reader | mobile | /vi/community | 32 | high | 0 | 30 | 0 |
| logged-in-reader | mobile | /vi/collection | 32 | high | 0 | 30 | 0 |
| logged-in-reader | mobile | /vi/reading/session/b48c44dc-9497-49af-8129-a32dd71f65ff | 31 | high | 0 | 29 | 0 |
| logged-in-reader | mobile | /vi/reading/session/b3918917-e26b-429d-b4ee-51dedf38e2cb | 36 | critical | 1 | 33 | 0 |
| logged-in-reader | mobile | /vi/reading/session/a9cf7586-252a-432a-bfc1-c0ecee4aac30 | 36 | critical | 1 | 33 | 0 |
| logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | 31 | high | 0 | 29 | 0 |
| logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | 31 | high | 0 | 29 | 0 |
| logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 31 | high | 0 | 29 | 0 |

## High Slow Requests (>800ms)
| Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-admin | desktop | /vi/reading/session/a193d0c6-9a62-42a1-8135-7631ad6d078f | GET | 200 | 4770 | 3742 | html | https://www.tarotnow.xyz/vi/reading/session/a193d0c6-9a62-42a1-8135-7631ad6d078f |
| logged-in-admin | desktop | /vi/community | GET | 200 | 3317 | 2905 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 1059 | 324 | html | https://www.tarotnow.xyz/vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 |
| logged-in-admin | desktop | /vi/reading/session/9e4d5054-f195-4546-b72a-e6d3b37b7939 | GET | 200 | 857 | 378 | html | https://www.tarotnow.xyz/vi/reading/session/9e4d5054-f195-4546-b72a-e6d3b37b7939 |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-admin | mobile | /vi/wallet/deposit/history | GET | 200 | 784 | 398 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | desktop | /vi/wallet/deposit/history | GET | 200 | 755 | 326 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 752 | 442 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-reader | mobile | /vi/reading/session/b48c44dc-9497-49af-8129-a32dd71f65ff | GET | 200 | 751 | 436 | html | https://www.tarotnow.xyz/vi/reading/session/b48c44dc-9497-49af-8129-a32dd71f65ff |
| logged-in-reader | mobile | /vi/reading/session/b3918917-e26b-429d-b4ee-51dedf38e2cb | GET | 200 | 746 | 343 | html | https://www.tarotnow.xyz/vi/reading/session/b3918917-e26b-429d-b4ee-51dedf38e2cb |
| logged-in-reader | desktop | /vi/wallet/deposit/history | GET | 200 | 745 | 339 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-reader | desktop | /vi/reading/session/59c2690a-55fc-42b5-9b56-b672ee0f49e5 | GET | 200 | 745 | 326 | html | https://www.tarotnow.xyz/vi/reading/session/59c2690a-55fc-42b5-9b56-b672ee0f49e5 |
| logged-in-admin | mobile | /vi/reading/session/b4d47003-af58-4b12-b2ae-381a7db36117 | GET | 200 | 734 | 314 | html | https://www.tarotnow.xyz/vi/reading/session/b4d47003-af58-4b12-b2ae-381a7db36117 |
| logged-in-reader | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 733 | 324 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 728 | 342 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-reader | mobile | /vi/reading/session/a9cf7586-252a-432a-bfc1-c0ecee4aac30 | GET | 200 | 716 | 338 | html | https://www.tarotnow.xyz/vi/reading/session/a9cf7586-252a-432a-bfc1-c0ecee4aac30 |
| logged-in-reader | desktop | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 711 | 326 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | mobile | /vi/wallet/deposit/history | GET | 200 | 703 | 411 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-reader | desktop | /vi/community | GET | 200 | 696 | 345 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | mobile | /vi/collection | GET | 200 | 693 | 384 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 690 | 324 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-reader | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 686 | 348 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-admin | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 676 | 328 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-reader | desktop | /vi/reading/session/88370553-4dca-4efb-ad03-037829d0e62b | GET | 200 | 670 | 394 | html | https://www.tarotnow.xyz/vi/reading/session/88370553-4dca-4efb-ad03-037829d0e62b |
| logged-in-admin | mobile | /vi/community | GET | 200 | 666 | 315 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | desktop | /vi/reading/session/367dda95-4b5a-48f9-8928-412c5a307556 | GET | 200 | 664 | 307 | html | https://www.tarotnow.xyz/vi/reading/session/367dda95-4b5a-48f9-8928-412c5a307556 |
| logged-in-admin | desktop | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 664 | 394 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-admin | mobile | /vi/reading/session/69338159-5cf0-40dc-ac0d-f3a699b43a23 | GET | 200 | 661 | 324 | html | https://www.tarotnow.xyz/vi/reading/session/69338159-5cf0-40dc-ac0d-f3a699b43a23 |
| logged-in-admin | desktop | /vi/reading/session/a193d0c6-9a62-42a1-8135-7631ad6d078f | GET | 200 | 653 | 316 | api | https://www.tarotnow.xyz/api/reading/cards-catalog |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 638 | 101 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | /vi/admin | GET | 200 | 636 | 312 | html | https://www.tarotnow.xyz/vi/admin |
| logged-in-reader | mobile | /vi/community | GET | 200 | 634 | 313 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 628 | 110 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 628 | 337 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 627 | 96 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 627 | 309 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-admin | mobile | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 623 | 418 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 622 | 102 | static | https://www.tarotnow.xyz/_next/static/chunks/0r68tfefeu~dz.js |
| logged-in-admin | mobile | /vi/collection | GET | 200 | 619 | 312 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-admin | desktop | /vi/community | GET | 404 | 616 | 601 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F1bf7374304584c0488e06621bbc1454f.webp&w=48&q=75 |
| logged-in-admin | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 616 | 325 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | mobile | /vi/reading/session/47bd41c1-1413-4d9e-ab1f-af581c197f93 | GET | 200 | 615 | 327 | html | https://www.tarotnow.xyz/vi/reading/session/47bd41c1-1413-4d9e-ab1f-af581c197f93 |
| logged-in-admin | desktop | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 609 | 314 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-admin | desktop | /vi/community | GET | 200 | 602 | 340 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-reader | mobile | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 599 | 323 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | desktop | /vi/community | GET | 200 | 598 | 590 | api | https://www.tarotnow.xyz/api/auth/session |
| logged-in-admin | desktop | /vi/admin | GET | 200 | 596 | 315 | html | https://www.tarotnow.xyz/vi/admin |
| logged-in-admin | desktop | /vi/community | GET | 200 | 583 | 576 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=32&q=75 |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 579 | 82 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 577 | 99 | static | https://www.tarotnow.xyz/_next/static/chunks/105-15pf-dz_l.js |
| logged-in-reader | mobile | /vi/reading/session/a9cf7586-252a-432a-bfc1-c0ecee4aac30 | GET | 200 | 573 | 308 | api | https://www.tarotnow.xyz/api/reading/cards-catalog |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 571 | 85 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 566 | 82 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 564 | 92 | static | https://www.tarotnow.xyz/_next/static/chunks/010jrdgfxuf04.js |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 564 | 327 | api | https://www.tarotnow.xyz/api/reading/cards-catalog |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 559 | 89 | static | https://www.tarotnow.xyz/_next/static/chunks/07cbcba783g-t.js |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 555 | 80 | static | https://www.tarotnow.xyz/_next/static/chunks/09njxa758vvw_.js |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 555 | 102 | static | https://www.tarotnow.xyz/_next/static/chunks/07tk2ft0d9n3x.js |
| logged-in-reader | mobile | /vi/reading/session/b3918917-e26b-429d-b4ee-51dedf38e2cb | GET | 200 | 551 | 308 | api | https://www.tarotnow.xyz/api/reading/cards-catalog |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 550 | 67 | static | https://www.tarotnow.xyz/_next/static/chunks/013oon8u7jcs1.js |
| logged-in-admin | mobile | /vi/reading/session/b4d47003-af58-4b12-b2ae-381a7db36117 | GET | 200 | 541 | 306 | api | https://www.tarotnow.xyz/api/reading/cards-catalog |
| logged-in-reader | desktop | /vi/reading/session/59c2690a-55fc-42b5-9b56-b672ee0f49e5 | GET | 200 | 538 | 306 | api | https://www.tarotnow.xyz/api/reading/cards-catalog |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 535 | 94 | static | https://www.tarotnow.xyz/_next/static/chunks/09ke324tyyggj.js |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 530 | 79 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 524 | 92 | static | https://www.tarotnow.xyz/_next/static/chunks/120f7usw0kcoa.js |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 503 | 92 | static | https://www.tarotnow.xyz/_next/static/chunks/0vsq~5x0h0e14.js |
| logged-in-admin | desktop | /vi/reading/session/9e4d5054-f195-4546-b72a-e6d3b37b7939 | GET | 200 | 470 | 108 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 465 | 80 | static | https://www.tarotnow.xyz/_next/static/chunks/0640_e4jatdk-.js |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 463 | 73 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 461 | 80 | static | https://www.tarotnow.xyz/_next/static/chunks/08nnjyw~vjmez.js |
| logged-in-admin | desktop | /vi/reading/session/9e4d5054-f195-4546-b72a-e6d3b37b7939 | GET | 200 | 459 | 93 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 457 | 68 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-reader | desktop | /vi/reading/session/b16a5530-8044-4e9a-913d-3c95a2c7c5f5 | GET | 200 | 452 | 82 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-0-hlnlst9psev.js |
| logged-in-reader | desktop | /vi/wallet/deposit/history | GET | 200 | 440 | 90 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | desktop | /vi/wallet/deposit/history | GET | 200 | 432 | 76 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |

## Pending Requests
| Scenario | Viewport | Route | URL |
| --- | --- | --- | --- |
| logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F06_The_Hierophant_50_20260325_181348.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=256&q=75 |
| logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F08_The_Chariot_50_20260325_181351.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=256&q=75 |
| logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F09_Strength_50_20260325_181351.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=256&q=75 |
| logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F10_The_Hermit_50_20260325_181353.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=256&q=75 |
| logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F12_Justice_50_20260325_181353.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=256&q=75 |
| logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F13_The_Hanged_Man_50_20260325_181356.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=256&q=75 |
| logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F14_Death_50_20260325_181356.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=256&q=75 |
| logged-in-admin | desktop | /vi/collection | https://www.tarotnow.xyz/_next/image?url=%2Fapi%2Fcollection%2Fcard-image%3Fsrc%3Dhttps%253A%252F%252Fimg.tarotnow.xyz%252Flight-god-50%252F15_Temperance_50_20260325_181356.avif%253Fiv%253D81a3d9698977fda2%2526variant%253Dthumb%26iv%3D81a3d9698977fda2&w=256&q=75 |

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
| logged-in-admin | desktop | reading.init.spread_3: created a193d0c6-9a62-42a1-8135-7631ad6d078f. |
| logged-in-admin | desktop | reading.init.spread_5: created 367dda95-4b5a-48f9-8928-412c5a307556. |
| logged-in-admin | desktop | reading.init.spread_10: created 9e4d5054-f195-4546-b72a-e6d3b37b7939. |
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
| logged-in-reader | desktop | reading.init.spread_3: created 59c2690a-55fc-42b5-9b56-b672ee0f49e5. |
| logged-in-reader | desktop | reading.init.spread_5: created 88370553-4dca-4efb-ad03-037829d0e62b. |
| logged-in-reader | desktop | reading.init.spread_10: created b16a5530-8044-4e9a-913d-3c95a2c7c5f5. |
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
| logged-in-admin | mobile | reading.init.spread_3: created 47bd41c1-1413-4d9e-ab1f-af581c197f93. |
| logged-in-admin | mobile | reading.init.spread_5: created 69338159-5cf0-40dc-ac0d-f3a699b43a23. |
| logged-in-admin | mobile | reading.init.spread_10: created b4d47003-af58-4b12-b2ae-381a7db36117. |
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
| logged-in-reader | mobile | reading.init.spread_3: created b48c44dc-9497-49af-8129-a32dd71f65ff. |
| logged-in-reader | mobile | reading.init.spread_5: created b3918917-e26b-429d-b4ee-51dedf38e2cb. |
| logged-in-reader | mobile | reading.init.spread_10: created a9cf7586-252a-432a-bfc1-c0ecee4aac30. |
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
