import React, { useEffect, useState } from 'react';
import MapView, { Callout, Circle, Marker } from 'react-native-maps';
import { StyleSheet, View, Text, Dimensions, Button, Pressable } from 'react-native';
import { GooglePlacesAutocomplete } from 'react-native-google-places-autocomplete';

import * as Location from 'expo-location';

export default function App() {

  const [location, setLocation] = useState({
    latitude: 0,
    longitude: 0,
  });


  useEffect(() => {

    getPermissions();
    //getCoordinates();

  }, [])

  const [pin, setPin] = useState({
    latitude: 45.7494,
    longitude: 21.2272,
  })

  const [region, setRegion] = useState({
    latitude: 45.7494,
    longitude: 21.2272,
    latitudeDelta: 0.0922,
    longitudeDelta: 0.0421,
  })

  const [coordinates, setCoordinates] = useState([{ archived: "false", id: 0, latitude: 0, longitude: 0, radius: 0 }]);


  const getPermissions = async () => {
    let { status } = await Location.requestForegroundPermissionsAsync();
    if (status !== 'granted') {
      console.log("Please grant location permissions");
      return;
    }
    let currentLocation = await Location.getCurrentPositionAsync({});


    console.log(currentLocation.coords.latitude, currentLocation.coords.longitude);
    setPin(currentLocation.coords);


  };

  const getCoordinates = async () => {
    try {
      const response = await fetch('https://apiambrosiaproject.azurewebsites.net/Info/AllPOI', {
        method: "GET",

      })
      const json = await response.json();
      setCoordinates(json);
      console.log(coordinates);

    } catch (error) {
      console.error(error);
    }
    getPermissions();
  }
  const postCoordinates = async () => {
    try {
      const response = await fetch('https://apiambrosiaproject.azurewebsites.net/Info/PostPOI', {
        method: "POST",
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ radius: 1500, latitude: pin.latitude, longitude: pin.longitude })
      });
      const json = await response.json();
      console.log(json);
    } catch (error) {
      console.error(error);
    }
  }


  return (
    <View style={{ marginTop: 40, flex: 1 }}>

      <MapView style={styles.map}
        initialRegion={{
          latitude: 45.7494,
          longitude: 21.2272,
          latitudeDelta: 0.0922,
          longitudeDelta: 0.0421,
        }} region={{
          latitudeDelta: 0.0922,
          longitudeDelta: 0.0421,
          latitude: pin.latitude, longitude: pin.longitude
        }} >



        {coordinates.map((marker, index) => (
          <Circle key={index} center={marker} radius={marker.radius} fillColor={'rgba(255, 0, 0,0.6)'} />
        ))}



        <Marker coordinate={pin} pinColor={"black"} draggable={true}
          onDragStart={(e) => {
            console.log("drag start", e.nativeEvent.coordinate)
          }}
          onDragEnd={(e) => {
            setPin({
              latitude: e.nativeEvent.coordinate.latitude,
              longitude: e.nativeEvent.coordinate.longitude
            }), console.log("drag end", e.nativeEvent.coordinate)
          }}
        >
          <Callout><Text>ME</Text></Callout>
        </Marker>

      </MapView>


      <GooglePlacesAutocomplete
        placeholder='Search'
        onFail={error => console.log(error)}
        onPress={(data, details = null) => {
          console.log(data, details);
          setPin({ latitude: details.geometry.location.lat, longitude: details.geometry.location.lng })
        }}
        fetchDetails={true}
        GooglePlacesSearchQuery={{ rankby: "distance" }}
        query={{
          key: "AIzaSyBl4i2vkcevEbR0YFgKxbZFRBcLjlzy-7U",
          language: 'en',
          radius: 3000,
          location: `${region.latitude},${region.longitude}`
        }}
        styles={{
          container: { flex: 0, position: "absolute", width: "100%", zIndex: 1 },
          listView: { backgroundColor: "white" }
        }}
      />

      <View style={{

      }}>
        <Pressable style={styles.button1} onPress={getCoordinates} >
          <Text style={styles.text}>Update</Text>
        </Pressable >
        <Pressable style={styles.button2} onPress={postCoordinates}  >
          <Text style={styles.text}>Send</Text>
        </Pressable >
      </View>
    </View >
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  map: {
    width: '100%',
    height: '100%',
  },
  button1: {
    marginLeft: 50,
    marginRight: 50,
    flex: 1,
    position: 'absolute',
    bottom: 10,
    alignItems: 'center',
    paddingVertical: 12,
    paddingHorizontal: 32,
    borderRadius: 4,
    elevation: 3,
    backgroundColor: 'black',
    width: "35%",

  },
  button2: {
    marginLeft: 220,
    flex: 1,
    position: 'absolute',
    bottom: 10,
    alignItems: 'center',
    paddingVertical: 12,
    paddingHorizontal: 32,
    borderRadius: 4,
    elevation: 3,
    backgroundColor: 'black',
    width: "35%",



  },
  text: {
    fontSize: 16,
    lineHeight: 21,
    fontWeight: 'bold',
    letterSpacing: 0.25,
    color: 'white',
  },
});