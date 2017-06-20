//Array Prototype Linq Library
//Author: Karim Korabi
//Copyright (c) 2016
var linq = {};
linq.forEach = function (action) {
    if (!action) return;
    for (var i = 0; i < this.length; i++) {
        if (action(this[i], i) === false) break; //break on return of false!
    }
};
linq.select = function (selector) {
    if (!selector) return this;
    if (this.length == 0) return [];
    var result = [];
    for (var i = 0; i < this.length; i++) {
        var temp = selector(this[i], i);
        if (Array.isArray(temp))
            result = result.concat(temp);
        else
            result.push(temp);
    }

    return result;
};
linq.where = function (predicate) {
    if (!predicate) return this;
    if (this.length == 0) return [];
    var result = [];
    for (var i = 0; i < this.length; i++)
        if (predicate(this[i])) result.push(this[i]);
    return result;
};
linq.first = function (predicate) {
    return !predicate ? (this[0] || null) : (this.where(predicate)[0] || null);
};
linq.last = function (predicate) {
    if (!predicate) return this[this.length - 1] || null;
    var tmp = this.where(predicate);
    return tmp[tmp.length - 1] || null;
};
linq.count = function (predicate) {
    if (!predicate) return this.length;
    return this.where(predicate).length;
};
linq.all = function (predicate) {
    if (!predicate) return true;
    return this.count(predicate) == this.count();
};
linq.any = function (predicate) {
    if (!predicate) return false;
    return this.first(predicate) !== null;
};
linq.sum = function (predicate) {
    var sum = 0;

    var action = function (item) {
        sum += item;
    };

    if (!predicate) this.forEach(action);
    else this.where(predicate).forEach(action);

    return sum;
};
linq.max = function (predicate) {
    if (predicate) return this.slice().where(predicate).sort(function (a, b) { return a < b }).first();
    return this.slice().sort(function (a, b) { return a < b }).first();
};
linq.min = function (predicate) {
    if (predicate) return this.slice().where(predicate).sort(function (a, b) { return a > b }).first();
    return this.slice().sort(function (a, b) { return a > b }).first();
};
linq.copy = function () {
    return this.slice();
};
linq.group = function (keySelector) {
    var groups = [];

    for (var i = 0; i < this.length; i++) {
        var key = null;
        var keys = [];

        for (var prop in keySelector) {
            var item = this[i];
            keys.push(item[prop]);
        }

        key = keys.join("|");

        var existing = groups.first(function (g) { return g.key == key });

        if (existing) {
            existing.values.push(item);
        }
        else {
            existing = {
                key: key,
                values: [item]
            };

            groups.push(existing);
        }
    }

    return groups;
};
linq.distinct = function (comparer) {
    if (this.length == 0 || this.length == 1) return this;

    var $self = this;
    var distinctValues = [];

    distinctValues.push($self.first());

    for (var i = 1; i < $self.length; i++) {
        if (distinctValues.any(function (d) { return comparer ? comparer(d, $self[i]) : d === $self[i] })) continue;

        distinctValues.push($self[i]);
    }

    return distinctValues;
};
Array.prototype.forEach = linq.forEach;
Array.prototype.select = linq.select;
Array.prototype.where = linq.where;
Array.prototype.any = linq.any;
Array.prototype.all = linq.all;
Array.prototype.first = linq.first;
Array.prototype.last = linq.last;
Array.prototype.count = linq.count;
Array.prototype.sum = linq.sum;
Array.prototype.max = linq.max;
Array.prototype.min = linq.min;
Array.prototype.copy = linq.copy;
Array.prototype.group = linq.group;
Array.prototype.distinct = linq.distinct;