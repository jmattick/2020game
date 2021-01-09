var covidlib = {
	$dependencies:{},
	RequestUnityJS: function(query){
		getCovidData(UTF8ToString(query));
	}	
};

autoAddDeps(covidlib, '$dependencies');
mergeInto(LibraryManager.library,covidlib);