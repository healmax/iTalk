﻿/* 自訂 filter 支持 dictionary (non-array) object 的搜尋 */
angular.module('customFilter', [])
    .filter('custom', function () {
        return function (input, search) {
            if (!input) return input;
            if (!search) return input;

            var propertyNames = Object.keys(search);
            var expected;
            if (angular.isArray(propertyNames) && propertyNames.length > 0) {
                expected = ('' + search[propertyNames[0]]).toLowerCase();
            }
            else {
                expected = ('' + search).toLowerCase();
            }
            var result = {};
            angular.forEach(input, function (value, key) {
                var actual;
                if (angular.isArray(propertyNames) && propertyNames.length > 0) {
                    actual = ('' + value[propertyNames[0]]).toLowerCase();
                }
                else {
                    var actual = ('' + value).toLowerCase();
                }
                if (actual.indexOf(expected) !== -1) {
                    result[key] = value;
                }
            });
            return result;
        }
    })

/**
 * Angular Image Fallback
 * (c) 2014 Daniel Cohen. http://dcb.co.il
 * License: MIT
 * https://github.com/dcohenb/angular-img-fallback
 */
angular.module('dcbImgFallback', [])
    .directive('fallbackSrc', function () {
        var missingDefault = "/Img/portrait.png";
        return {
            restrict: 'A',
            link: function (scope, element, attr) {
                // Listen for errors on the element and if there are any replace the source with the fallback source
                var errorHanlder = function () {
                    element.off('error', errorHanlder);
                    var newSrc = attr.fallbackSrc || missingDefault;
                    if (element[0].src !== newSrc) {
                        element[0].src = newSrc;
                    }
                };
                element.on('error', errorHanlder);
            }
        };
    })
    .directive('loadingSrc', function () {
        var loadingDefault = "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQEAYABgAAD/4QBoRXhpZgAATU0AKgAAAAgABAEaAAUAAAABAAAAPgEbAAUAAAABAAAARgEoAAMAAAABAAIAAAExAAIAAAASAAAATgAAAAAAAABgAAAAAQAAAGAAAAABUGFpbnQuTkVUIHYzLjUuMTEA/9sAQwAEAgMDAwIEAwMDBAQEBAUJBgUFBQULCAgGCQ0LDQ0NCwwMDhAUEQ4PEw8MDBIYEhMVFhcXFw4RGRsZFhoUFhcW/9sAQwEEBAQFBQUKBgYKFg8MDxYWFhYWFhYWFhYWFhYWFhYWFhYWFhYWFhYWFhYWFhYWFhYWFhYWFhYWFhYWFhYWFhYW/8AAEQgB9AH0AwEiAAIRAQMRAf/EAB8AAAEFAQEBAQEBAAAAAAAAAAABAgMEBQYHCAkKC//EALUQAAIBAwMCBAMFBQQEAAABfQECAwAEEQUSITFBBhNRYQcicRQygZGhCCNCscEVUtHwJDNicoIJChYXGBkaJSYnKCkqNDU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6g4SFhoeIiYqSk5SVlpeYmZqio6Slpqeoqaqys7S1tre4ubrCw8TFxsfIycrS09TV1tfY2drh4uPk5ebn6Onq8fLz9PX29/j5+v/EAB8BAAMBAQEBAQEBAQEAAAAAAAABAgMEBQYHCAkKC//EALURAAIBAgQEAwQHBQQEAAECdwABAgMRBAUhMQYSQVEHYXETIjKBCBRCkaGxwQkjM1LwFWJy0QoWJDThJfEXGBkaJicoKSo1Njc4OTpDREVGR0hJSlNUVVZXWFlaY2RlZmdoaWpzdHV2d3h5eoKDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uLj5OXm5+jp6vLz9PX29/j5+v/aAAwDAQACEQMRAD8A+ggKcBQBRQAUAZpQKUUAFOAxRSgUAIBThQBTqAAClAoApwFACU4CilAoAQCnYoxTsUAJilpcUtACYpaXFLQAmKXFLiloAbilxTsUuKAG0YNOowaAExS4FLilxQA2inYooAbRg07BpcGgBmDRg0/FGKAGYNFPxSYNADaKdg0UANxSYp+BSYoAbikwafikoAbikxT6TFADMUYp2KMUAR4oxT8UmKAGYpMU/FJigCPFJin4pMUAMpCKfSEUAMIppFSEU2gBhFIRTyKaRQAykIp9NoAaRmm08ikNADaQilooAbiilxRQAtKBSAZpwoABTqKUCgAApwFAFKKAACnAUU4CgBAKUClApQKAClApaUCgBMU7FGKdigBMUuKUClxQAmKXFLiloATFLS4paAG4NLinYpcUANpcUtLigBuKXAp2KKAG0YNOooATFGKWigBMUYpaKAExRilooATBpMGnUUANowKdRigBmKTBp+KMUAMxSYp9JigBlGKdijFADMUmKdiigBmKTFPxSYoAjxSYp+KTFADKQin00igBpFNxTyKQigCMikIp5FIRQAwim08ikNAEZGKQin00jFADTTaeRSEUANooooAdTqAMUoFAABTgKAKUUAAFOopwoAAKUCgCnAUAAFLQBTqADGKUClApQKAEApwFFOAoATFLRinAUAJilpcUuKAExS0uKWgBMUuKKKACijBpcUAJRTqMGgBMGjFOxRigBuKXAp2BRigBuBRgU6igBuBSYp9GBQAzFGKfgUmKAGYNFPxSYoAbRTsUmKAEpMUtFADcUYp1GKAI8UYp+KTFAEeKMU7FJigBmKQin0hFADKaRipCKaaAGEUhFPIpCKAI6aRUlNNADCKQjNPIppFADCKbUhFNNAEZGKQinn0ptADaKXFFACgU4CgU4UAAp1ApwFAAKUChRTgKAAClAzQBmnAUAFOFAFOAoAQClApQKWgApQKUClAoAQCnYop2KAExS0UUAFFLiloATFLS4paAExS4pcUuKAG0uKXBpcUANxS4FOxRigBtFOowaAG0U7BowaAG0U7BowaAG4oxTqMCgBmKTBp+KMUAMpMU+kxQAzBoxTsUYoAZikp+KSgBtGKXFJQA0ikxT6QigBlNIqTFNIoAaRTaeRSUARkUhFPIpCKAGEZptPIpCKAIzSEU+mmgBjCmkU8jFIwoAYRmmmnsKawoAZRTqKAHAUqihRTgKABR3pyikApwoAUClAoFOoAKcBQKcBQAAYpQKAKWgApwFFOAxQAgFOAoApaACiilAoAAKWgCnUAJilxS4paAExS4pcUuKAExS0uKWgBuDS4p2KMUAJgUU6igBuDS4pcGlwaAG4oxTsUYoAbijBp2KTBoAbg0U7BooAbgUmKfikxQAzFGKdg0YoAZikxT8UlADMUmKfikoAZikxTyKQigCOinUhFACYzTadRQAwimkU8jFIRQAymkYp5FJQAwimkU8jFIRQAwjNNNPYU0igBh9KaakIppoAjPFNIqQ02gBmKKWigBwp1ApVoAUU4UiinKKAFHFOApFFOUUACinAUgFOoAKdQBTgMUAAGKUCgCloAKKAM07FAAKUClAxSgUAJinUAU7FACYpaXFLQAYoxTsUUAJilpcUtADcGlxTsUYoATFGDTqMGgBMGjFOxRigBuKMU7FGKAG4oxTsUYoAZg0U/FJg0ANxSYp+KTFADKKdg0YoAZikp+KSgBmKSn4pKAIyKQin4pCKAGU0ipKaRigBhFJTyKQigBtNIxTqKAGEU0inkYpCKAGU008ikoAjIxSMKeaaaAGMKawp5ppFADGFNanmmmgBtFLiigBRThQKcKAAU4UCnCgAFOFApwoAKcBSKKcooAAKcooApaACgDNFOHpQAU4UClAoAAKcBQBSgZoAAKcBRinAUAIBS0YzTsUAJilpcUtACYpaXFLigBMUuKXFLigBuKXFLg0uKAG4pcCnYoxQA3AowKdRQA3ApMU+jAoAZijBp2KMUAMpMU/FGKAI8UYp+KTFADMUmKfikxQBHiinYpMUAMxSEU+kIoAYRTSKkIptADCKaRTzSEUAMIzTaeRSEUANppp1FAEZFIwp9NNADSKaacaRhQAw0009qa1ADDTTT29aa1ADKKdiigBVpy0lOoAVactIKcKAFWlFFOFAAKcKBThQAUUUq0AKKcBikUU5RQAAU4ChRSgZoAUClAoAp1ABSgUAUoFAAKcBRTgKAEApaUCloATFLilxS4oATFLS4pcUAJijFOxS4oAbiinUYNADaKdg0YNADaKdg0YNADcCkxT8UmKAGYNGKfikoAZikp+KSgBmKTFPxSEUAR4pCKeRSEUAMptSEZptADCKQinEYpCKAGEU01IRTaAIyKRhT6aaAGMKSnGmmgANNNOpGoAYabT2prUAMNNNPamtQAw0009qa1ADKKdRQAq05aSnCgBVpy0gp1ACrTlpKcKAFWloooAVaUUU4UAApwoFOFAAKcPSgU4CgApQKFFKBQAoFKKBTgKAClApQKUCgAApaAKdQAmKXFLilxQAYoxTsUYoATFLilxS0ANwaXBpaKAExRilooATFJg06igBtGKdRigBmKSn4pKAGYpKfikoAZikxT8UhFAEZFIRTzTSKAGEUhp5FNIoAZTSMVIaaaAGEUhFOpGFADDTTT2pretADDTae1NagBlFK1JQA00009qa1ADKaae1IaAIzTTUjUxqAGUU6igBVpy0gp1ACrTlpKcKAFWnLSU6gApVpKdQAq05aQU4UAKtOX1pBThQAq0o5opwoABTqBThQAU4CkApwFAABTgKAKUDNAABTqKcBQAgFLilApaADFFFFABRS4paAG4NLinYNGKAG4oxTsUYoAbijBp2KMUAMop2DRigBtFLikoATFJTqKAGEU0inkUhFADCKaakIzTaAGEU0inkU0jFADGFIRTiKRhQAwim09qa3rQAw001IaaaAIzTTUhppoAjNNNSNTWoAbTTTqRqAGU2ntTWoAYaaae1NagBlFOxRQALT1pq09elAAKetNWnrQAq0tA6UUAKtOWkFOFACrTlpBThQA5actNFOoAVactJTqAFWnLSU4UAKopQKKdQACnUCnCgAFKBQBS0AFFFOAoAQClxSgUuKAExS4p2KMUAJijFOxS4FADcCjAp1FADcCkxT6MUAMxSU/FJQAzFJT8UlADCKSnEUhFACUhFLRQAwikIp5FNYUAMNNp7CmtQAymmntSNQBGaaakamtQBHSNTmpDQAxqYakNNNAEZppp7U1qAGUUrUlADTTTT2prUAMpppx60jUAMooPWigB1Opq9acOtADqcKRactAC0L1opVoActOWkXpTh0oAVactIKcKAFWnLSU4UAKtOWkpwoAVactIKcKAFWnLSCnCgBVpyikFOoAKKKcKACnAUAUoFAABS4pQKWgAxRinYoxQAmKWnYooAbRg07BpcGgBlGKdg0UAMxSU/FJQAzFJTyKQigCMikIp9NIxQAwikp7CmsKAEppp1BoAjNNNSGmmgCM001IaaaAIzTTUhpjUAMNNp7U1qAGNTTT2prUAMNNp7U1qAGGm09qYetAA3SmNT6bQAxqa1PNNoAbRRRQAq05aRelOXpQAq09elNHSnUAFOFNFOHWgB1OHWkWnLQAq9aetNWnLQAq9aetNWnLQA5actIOlOFACrTlpBThQA5acKaKcKAHCiiigBVpy0gpwoAVRTgKQU4UAAFOFApwoAAKMUoFKBQAYowadijBoATFGKdijFADcGkp+KTFADMUlPxSUAMIpCKeRTWFADCKbTyKQigCM001IaaaAIzRTjTaAEamtT6aaAGNTGqQ0xqAGGmmntTWoAZTTT2pjdaAG00049aa3WgBpprdKcetNNADGprU8009KAG0jdaWkagBh602ntTW60AMNFK1FAC04U2nUAOp1NHWnUAKvWnLTVpy0AOWnr0pq9KcKAHDpTqbTqAHCnU0dacvWgBwpy9aRactADlpy01aevSgBVpy0gp1ABSrSU6gBVp600U4UAOFOFNFOWgBwrUt9GlkgSUTIN6hgCDxkVlrXU27FNHjdeqwAj/vmgDN/sOX/nun5Gl/sSX/AJ7p+RqMaxdntH/3zS/2vd+kf/fNAEn9iy/89k/I0f2LL/z2T8jTP7WuvSP/AL5pf7Vu/SP/AL5oAf8A2NJ/z2X8jR/Y0n/PZPyNN/tW6/6Z/wDfNH9qXXon/fNADv7Fl/57L+RpP7Gl/wCeyfkab/at1/0z/wC+aT+1br0j/wC+aAHf2LL/AM9k/I0f2JL/AM90/I03+1rv0j/75pP7Xu/SP/vmgB39hzf890/I1W1LTXs4BK0isGbbgD2P+FWrPVLmW7jjYR7WYA4WrHij/kHp/wBdR/I0Ac81Nan000AMamtTzTTQBG1NNPamtQAlNbrTqRqAGGmmntTWoAYaaelOPWmmgBrdKY1Ppp6UAMamtTm6UjdKAGNTW605qa1ADD1ptPamHrQA2hulFBoAY1Nant0pjUAJRRRQAU5etNHWnr1oActLSLS0AKtPXpTV6U4UAOp1Npw60AOXrTh1pFpy9aAHL1py01actADlp69KavSnDpQA4dKdTacOtADh1p1IvWloAB1p69aatOWgBy05aatPHSgBVp602nUAOFdNH/yBF/69x/6DXMjrXTR/8gVf+vcf+g0Ac6KcKRaUUAOjQuwVQST0ArVs9LULvuT/AMBB/mafpsEdna/aZ/vEZ+g9PrVK9vJbliM7U7KP60AaIn0+3O1TGp/2Rn9RThf2bceYPxU1i4ooA2pLOzuEyqrz/ElZeoWElv8AMPnj/vDt9aZBLJC++Jip7jsa2bG5S7hOQNw4dTQBzp4pGq9q1p9nmyg/dv8Ad9vaqVAEmm/8hCH/AK6D+danij/kHp/11H8jWZpv/IQh/wCug/nWn4o/5B6/9dR/I0AYDU1qe3SmNQAw9aaae1NagBlNPSnN1ppoAbQelFFADW6UxqeelNbpQAxqa3WnNTWoAYetNp7daYetADT0ptOptADW6Uxqeaa3SgBjU1qc1NagBjdaKVqSgBtNbpTjTT0oAbRRRQAL1p60xetPWgBy0tItLQA5elOpq9KdQA6nL1ptOXrQA9actNWnLQA5actNWnLQA9elOpq9KdQA6nL1ptOXrQA9aWkWloAVactNWnLQA9elOpq9KdQA6nL1po609aAHLXSx/wDIFX/r3H/oNc0tdLH/AMgVf+vcf+g0Ac8tWdLi868RD0zk/QVXXpV/w7/x/H/cP9KAJNdnLXAhB+VOv1qkKddktdSE/wB8/wA6bQA4CiilJoAawqSymMF0smeM4b6VG1NagDd1OITWTrjJA3L9RXOnrXT253W6HPVR/Kuak4kIHrQA/Tv+QhD/AL4rU8Tf8eCf9dR/I1maf/yEIf8AfH860/E3/Hgn/XUfyNAGAaa3SnGmnpQAxqa1OamtQA1qY3WntTG60ANooooAbTW6U401ulADGprU5qa1ADWpjdae1MbrQA2m06m0ANprdKdTW6UAMamtTmprUANakpWpKAG02nU2gBtFFFAAvWnrTKcvWgB60tItLQA5elOpi9KeOlADqcvWm04daAHrTlpq9aVetAD1py01actAD16U6mLTx0oAdTl602nCgB60tNXrTqAFWnLTV605aAHr0pwpi09elADqcOtNHSnUAPWulj/5Aq/9e4/9BrmRXTR/8gVf+vcf+g0Ac8tXdEkEeoLno2VqiKepKsGBwQcg0AWdSj8u+kX1bI/HmolNX7oC+s1uY/8AWRjDrWeDQA8GjNNzRmgBSaaaM0sal2wOB1J9BQBILi4S38tZWCt29qrU+ZgW+Xp0A9qYaAJdO/5CEP8AvitTxN/x4J/11H8jWVpv/IQh/wCug/nWp4o/5B6f9dR/I0AYJprdKVqa1ADWprU5qa3WgBrUw9acetNoAbRRRQA2mt0px6UxqAGtTWpzU1qAGtTG605utNNADabTqbQA2mt0px6U1ulADGprU5qa1ADWpKG60UANptONNbpQA2iiigApw602nUAPXrS00dadQAq09elMWnLQA8dKdTV6U4dKAHDrThTadQA5etPWmU4daAHrTlpq9actAD16U4UxactAD6dTV6U4UAFOHWm04UAPWnLTBTqAHrTlplOFAD1rp4/+QGv/AF7j/wBBrlxXURc6IuP+fcf+g0Ac4tOBo8qX/nm//fJpfLl/55v/AN8mgCWzuJLaXfGfqD0NXGjtr354HEMp6o3Q/SqAjk/55t/3zR5cn/PNv++aAJ5rW5iOGhb6gZH6UwRyngRsf+AmnRyXkYwjTADtzStPfN1eb9aAD7M6jdOwiX/a+9+VMmlXb5cQ2p3z1b60xklJyUc/gaTy5P8Anm3/AHzQA2mk08xy/wDPNv8AvmkMcv8Azzf/AL5NAEmm/wDIRh/66D+danij/kHp/wBdR/I1mabHINQhJjYfOO1afir/AJB6f9dR/I0Ac+aaacaa1ADTTaVqa1ACGmnpStTWoAShulFI1ADWprU5qYetACNTG60402gBpptOPSmt0oAaelNPSlamt0oARulManNTWoAa1NbrTm60w9aAGnrRRQelADT0prdKc3SmNQAlFFFAAKcOlMXpTloAfTqavSnDpQAL1p60ynCgB605aYOtPWgB69KcOlMWnLQA8dKcKYtOWgB9OFMWnigBw609aYKcKAHrTlplOoAdSrSUUAPWnLTKcKAHrTlNMFOBoAeproLPVLOOzijaRtyxqD8p6gVzwNKDQB0v9rWP/PVv++DS/wBrWX/PRv8Avk1zQNOzQB0f9q2X/PRv++TR/atl/wA9G/75Nc7mlzQB0X9qWf8Az0b/AL5NH9qWf/PRv++TXO5NGTQB0X9qWf8Az0b/AL5NJ/atl/z0b/vk1z1GaAOh/tWy/wCejf8AfJpP7Wsf+ejf98mudzQTQB0X9rWP/PRv++DVHXr+2ubNY4XJYSAnKkcYP+NZOaaTQAE000pNNY0AIaaaVjTWoAQ02lakoAKaac1MagBKaaVqa1ACHpTGpzU1qAGtTWpW6000AI1Nalpp60AI3WmHrTqaaAG00049Ka3SgBtI3SlpGoAa1NanN1ph60AFFNbrRQAq05aYOtOoAetOWmDrT1oAWnDpTaVaAHinCmLTloAeKcvWminCgB1OHWm04UAOWnrTBThQA9actMFOoAetOWmU6gB60tNp1ACqaUGm0qmgB4NOpgNKDQBIDSg0ynA0APBpc0zNLmgB+aXNMzRmgB+aM03IozQA7NJmkyKTNAC5ozTc0ZoACaQmkzSE0ABNITQTTaAAmmmgmmk0AFFFIxoAQ000rU1qAENNNK1NagBDTTStTWoAQ009KVqa1ACU09KVqa1ACN0pjU5qY1ACNTWpTTaACmnrTqaaAGmm049KaaAG0UUUAAp1MWnLQA8U4UxactAD6KRaWgBw605aaKcKAHrTlpgpwoAetOWmU6gB605aYKcKAHrTlpgpwNAD1py0wU4GgB4NKDTAacpoAfRSA0tACg04GmUoNAD807NRg07NAD80uaZmlzQA7Jpc0zNLmgB2aM03NGaAHZozTc0ZNAC5pM0maTNAC5pCaTNJmgBSaaTRmmk0AKTSUUUABppNFNJoAKbQTSMaAEptKxprGgBDTTStTWoASm0rU1qAENNpWprUAIaaaVqa1ACHpTaVqSgBGprUrdaaaAEamtS009aAEJopKKAAU4U1acKAHU4U0U5aAHCnU0U4UAKtOWmU4UAPWnLTBThQA9actMFOFADhThTRSrQA8U4GmKacpoAeDTgaYppQaAJAacDUYNOoAeDTgaYDSg0APopoNOzQAZp2abRQA7NLmmZpc0APoyabmjJoAfmjNMyaMmgB+TSZNNyaM0AOzSZpM0maAFzSZpKKACiikJoAXNNoJppNAATSE0E00mgAY0hNBNNoACaaTRTSaAA000GmmgApppWprUAIabStTWoAQ02lamtQAlFFI1ACGmt0pWprUAIaaaVqa1ACUUhNFACLT1pgpwoAetKKaKdQA4U9aYKcKAHUq0gooAetOU0wU4GgBwNOFMU05TQA8U6mKacpoAeKcDUdOoAeDTlNMBpQaAJAaUGmA0oNAEgNKDTKXNAD807NR5p2aAH5paZmlzQA6im5NLmgBaKMiigAzRmiigAooooAKKMikzQAtGabmkzQAuaTNJmkzQAtNJopCaAAmkJoJptABTSaCaQmgAJprGgmkJoARjSGimmgANNNBNNNAAaaaCaRjQAhptK1JQAU00rU1qAENNpWprUAJTaVqa1ACUUmaKABactMFOFAD1py0wU4UAOFOFNFKtADwadTFpymgBacKbSqaAH04VGDTgaAJAacDUYNOoAeppQaaDSg0APpwNRg04GgB4NKDTQaUGgB9KDTM07NADs07NR5p2aAH5pc1HmlzQA/NLmmZpc0APzRmmZpcigB2aM+9NyKMigB2aMim5FJmgB2aM03NJk0AOpM0maTNAC5pM0maTNAC0hNJSE0AKTTaCaaTQApNNJoJpCaAAmm0UhNACE0jGhjTSaABjTWNKTTSaAEY0hNBNNoAKDRTSaAA000GmmgAptK1NagBKaaVqa1ACUUmaKAEFOFMWnLQA8U4UxactADxTqYppymgB4pwqMGnA0ASA0U0U6gBVNOU0ynA0APBpQaYDTgaAHg04GowadQA8GlpoNKDQA8GlBplLmgB+admo807NADs07NR5pc0APzS5pmaXNAD80ZNNzRmgB+aM03JozQA7NGabmjNADs0ZpuaM0AOzSZpuTRmgB2aTNNzRmgBc0maTNJmgBc0maTNJmgBc03NGabmgBSaSjNNzQAE0hNBNNJoACaQmgmm0ABNNJoJpGNACE0UUhNAAxprGlJppNACMaQ0U0mgANNNBppoADTTQaaaACikJooASnCmKacpoAfTqYppQaAJBTgajBpwNAEgNKpplOoAeDSg0xTTgaAH0U0GnUAOBpQaZTgaAHA06mA0oNAEgNKDTAaXNAD807NR5p2aAHUuabmlzQA7NLmmUuaAH0uaZmjJoAkzRTM0uRQA7JpcmmZoyaAH5ozTMmjJoAfmkyabk0ZNADs0ZpuaMigBc0mTSZpM0AOzSZpM0maAFpM0lJmgBaTNJmkzQAuaaTRmm5oAUmkJpCaQmgAJppNBNITQAE0lFBNAATTSaKaTQAE02gmkY0ADGmsaUmmk0AIxprUpNNNACMaRqKaaACim0UAANOBqMGnCgCQGlU0wU4GgB6mnKaYDTgaAHA04GmA05TQA+lBpgNOoAeDSg0wGnA0APopoNOoAUGlBptANAEmaUGmUoNAD807NR5p2aAH5pc1HmnZoAdmlzTM0uaAH0ZNNzRk0APzRmm5pcigB2aM03NFADs0ZptFADs0ZptFADsikzSZpM0AOzSUmaTJoAdSZpM0maAFzSZpKTNAC5pM0lJmgBc00mjNITQAE0hNBNNoACaKKQmgBSabQTTSaAAmkJoJpCaAAmmk0U2gAppNBNITQAhNNJpWNNY0ADGmsaGNITQAmaKSigBFNOU0wGlBoAeDTgaYppwNAD6cKjBpwNAEgNKpplOBoAeDSg0wGnA0APpQaYDTqAHA06mA0oNAEgNFNBpc0ALS5pKKAHZp2ajzTs0AOzS5pmaXNAD80uaZS5oAfmjJpuaM0APzRmm5NGaAH5FGaZmlyKAHZozTcijIoAdmjNNyKM0AOyKTNNzRmgB2TSU3JozQA7NJmm5pM0AOzSZpM0maAFzSZpKTNAC5pM0lFABRRmmk0AKTSE0hNITQAE00mikJoAUmm0E02gAJppNBNITQAE01jQTSE0ABNNJoppNABTaCaaTQAUUmaKAEpwpimlFAEgNKDTKcDQA9TTgaYDSg0APBp1MBpQaAJAaUGmA0oNADwacDTAaXNAEgNGabmlBoAeDSg0ylBoAfmlzTM0uaAH0U3NLmgBaXNJRQA7NLmmUZNAD80uaZmlyKAHZNLk0zNGTQA/NGabmjNADs0ZpuaM0AOzRmm5ozQA7NGTTcmkyaAHUZpuaM0ALmkzSZpKAHZpM0lFABRRSZoAWkzSZpM0ALmm5ozTc0AKTSUZppNACk0hNITSE0AFNJoJpCaAAmmk0E0hNAATTaKaTQAE0hNDGmk0ADGmsaUmm0AFFNzRQAA0qmmU4GgBwNOpgNKDQBIDTgajpwNADwaUGmA04GgB+acDUYNOBoAeDS5pgNKDQBIDSg0zNLmgB+aXNMzS5oAfS5pmadmgB1LmmUuaAH5pc0zNLmgB+aKZmlzQA6ikyaM0ALRRkUZoAMmlyaSigBc0ZpKKAFzSZNFFABk0UUZoAKKTNGaAFopuTRmgBc0mTSZpM0AOzSZpM0maAFzSZpKTNAC0maSkzQAuabmjNNzQApNITSZpM0ABNITSE0hNACk02gmmk0ABNITQTTSaAAmkJoJptABTSaUmmsaADNFJRQAUUimloAcDSg0ynA0AOBp2aYDS0ASA0oNMBpQaAH5pwNRg06gB4NLmmA0oNAEmaXNMzS5oAfmlzTM0uaAH5p2ajzS5oAfmlzTM0uaAH0ZNNzS5oAdmlzTM0tADsmlzTKXJoAdmlyKZmjNAD80ZpuRRkUAOzRmm5ozQA7NGabmjIoAdmjIpuRSZoAdmjJpuaTJoAdmjNNozQAuaSkzSZNADs0maTNJmgBaTNJSZoAWkzSZpM0ALmm5ozTc0AKTSUZpuaAFJppNBNITQAU0mimk0AKTSE0hNJQAUhNITSE0ABNJRRQAUU2igAWlWiigBaKKKAHUq0UUALTqKKAFWloooAdSrRRQAtOFFFABTqKKACnUUUAFOFFFABRk0UUAOooooAM0uaKKAFooooAKKKKACiiigAooooAKKKKAA03JoooAKKKKAEJpKKKACkJoooASkaiigBKRqKKAEpGNFFACU2iigBGpKKKAG0jUUUAJTTRRQAjUlFFABTaKKAEJooooA//2Q==";

        // Load the image source in the background and replace the element source once it's ready
        var linkFunction = function (scope, element, attr) {
            element[0].src = attr.loadingSrc || loadingDefault;
            var img = new Image();
            img.src = attr.imgSrc;
            img.onload = function () {
                img.onload = null;
                if (element[0].src !== img.src) {
                    element[0].src = img.src;
                }
            };
        };

        return {
            restrict: 'A',
            compile: function (el, attr) {
                // Take over the ng-src attribute to stop it from loading the image
                attr.imgSrc = attr.ngSrc;
                delete attr.ngSrc;

                return linkFunction;
            }
        };
    });

var iTalkApp = angular.module('iTalkApp', ['SignalR', 'matchmedia-ng', 'ui.bootstrap', 'ngMaterial', 'ngMessages', 'customFilter', 'dcbImgFallback']);

iTalkApp.controller('indexController', ['$scope', '$http', '$mdSidenav', 'Hub', 'matchmedia', function ($scope, $http, $mdSidenav, Hub, matchmedia) {
    var isInit = 3;

    matchmedia.onPhone(function (mediaQueryList) {
        $scope.isPhone = mediaQueryList.matches;
    });

    $scope.users = {};
    $scope.friends = {};
    $scope.groups = {};
    $scope.chats = {};

    // 目前對話的朋友或群組
    $scope.current = null;

    $scope.initUser = function (username) {
        $http.get('/account?userName=' + username)
            .then(function (response) {
                $scope.me = response.data.result;
            }).finally(function () {
                loadingComplete();
            })
    }

    $http.get('/friend')
        .then(function (response) {
            angular.forEach(response.data.result, function (f, i) {
                $scope.friends[f.id.toString()] = f;
                $scope.chats[f.id.toString()] = null;
            });
            loadingComplete();
        }, function (response) {
            showError(response.data);
            loadingComplete();
        });

    $http.get('/group')
        .then(function (response) {
            angular.forEach(response.data.result, function (g, i) {
                $scope.groups[g.id.toString()] = g;
                $scope.chats[g.id.toString()] = null;
            });
            loadingComplete();
        }, function (response) {
            showError(response.data);
            loadingComplete();
        });

    function loadingComplete() {
        isInit--;

        if (!isInit) {
            angular.element('#loading-modal').modal('hide');
        }
    }

    $scope.tabIndex = 2;

    $scope.setTabIndex = function (index) {
        $scope.tabIndex = index;
    };

    $scope.isCurrentTab = function (index) {
        return $scope.tabIndex === index;
    };

    $scope.isSender = function (chat) {
        return chat.senderId === $scope.me.id;
    };

    /**
    * @id friend or group id
    * return web api controll name
    */
    $scope.getControllerName = function (id) {
        return $scope.current.id > 0 ? 'chat' : 'groupChat';
    };

    $scope.setCurrent = function (target) {
        $scope.current = target;

        if (!$scope.chats[target.id.toString()]) {
            $scope.isloading = true;

            $http.get('/' + $scope.getControllerName() + '?targetId=' + target.id)
                .then(function (response) {
                    $scope.chats[target.id.toString()] = response.data.result;

                    if ($scope.current.id < 0) {
                        $http.get('/group?groupId=' + $scope.current.id)
                            .then(function (res) {
                                angular.forEach(res.data.result, function (u, i) {
                                    $scope.users[u.id.toString()] = u;

                                    if ($scope.friends[u.id.toString()]) {
                                        $scope.friends[u.id.toString()] = u;
                                    }
                                });
                            })
                    }
                }, function (response) {
                    showError(response.data);
                }).finally(function () {
                    $scope.isloading = false;
                })
        };
    };

    $scope.isSameDay = function (date1, date2) {
        return new Date(date1).toLocaleDateString() == new Date(date2).toLocaleDateString();
    }

    $scope.toLocaleDateString = function (date) {
        return new Date(date).toLocaleDateString();
    }

    $scope.showError = function (result) {
        alert(result.statusCode + ' : ' + result.message);
    }

    $scope.count = function (dict) {
        return Object.keys(dict).length;
    }

    $scope.detail = null;

    $scope.showDetail = function (navId, id) {
        if ($.isNumeric(id)) {
            if (id > 0) {
                $scope.detail = $scope.users[id];
            }
            else {
                $scope.detail = $scope.groups[id];
            }
        }
        else {
            $scope.detail = $scope.current;
        }

        $mdSidenav(navId).toggle();
    };

    // SignalR
    var hub = new Hub("chatHub", {
        listeners: {
            'receiveChat': function (friendId, chat) {
                $scope.chats[friendId.toString()].push(chat);
                //angular.forEach($scope.friends, function (f, key) {
                //    if (f.id == chat.targetId) {
                //        f.chats.push(chat);
                //        return;
                //    }
                //});
                $scope.$apply();
                window.scrollTo(0, document.body.scrollHeight);
            },
            'receiveGroupChat': function (groupId, chat) {
                $scope.chats[groupId.toString()].push(chat);
                $scope.$apply();
                window.scrollTo(0, document.body.scrollHeight);
            }
        },
        //methods: ['send'],
        errorHandler: function (error) {
            console.error(error);
        },
        hubDisconnected: function () {
            if (hub.connection.lastError) {
                hub.connection.start();
            }
        },
        //transport: 'webSockets',
        logging: true
    });

    //var serverTimeHubProxy = signalRHubProxy(
    //    signalRHubProxy.defaultServer, 'serverTimeHub');

    //clientPushHubProxy.on('receiveChat', function (chat) {
    //    angular.forEach($scope.friends, function (f, key) {
    //        if (f.userName == chat.sender) {
    //            f.chats.push(chat);
    //            return;
    //        }
    //    });
    //});

    //$scope.getServerTime = function () {
    //    serverTimeHubProxy.invoke('getServerTime', function (data) {
    //        $scope.currentServerTimeManually = data;
    //    });
    //};
}]);