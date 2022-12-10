function convertFirstToUpper(text) {
    return text.charAt(0).toUpperCase()+text.slice(1);
}

function convertToShortDate(datestring) {
    const shortDate = new Date(datestring).toLocaleDateString('tr-TR');
    return shortDate;
}