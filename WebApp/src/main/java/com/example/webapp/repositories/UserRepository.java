package com.example.webapp.repositories;

import com.example.webapp.models.UserEntity;
import com.mongodb.ReadConcern;
import com.mongodb.ReadPreference;
import com.mongodb.TransactionOptions;
import com.mongodb.WriteConcern;
import com.mongodb.client.MongoClient;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.model.FindOneAndReplaceOptions;
import jakarta.annotation.PostConstruct;
import org.bson.types.ObjectId;
import org.springframework.stereotype.Repository;
import java.util.ArrayList;
import java.util.List;
import static com.mongodb.client.model.Filters.eq;
import static com.mongodb.client.model.ReturnDocument.AFTER;

//UserRepository
@Repository
public class UserRepository implements IUserRepository{
    private static final TransactionOptions txnOptions = TransactionOptions.builder()
        .readPreference(ReadPreference.primary())
        .readConcern(ReadConcern.MAJORITY)
        .writeConcern(WriteConcern.MAJORITY)
        .build();
    private final MongoClient client;
    private MongoCollection<UserEntity> userCollection;

    public UserRepository(MongoClient mongoClient) {
        this.client = mongoClient;
    }

    @PostConstruct
    void init() {
        userCollection = client.getDatabase("tinyWhatsApp").getCollection("users", UserEntity.class);
    }

    @Override
    public UserEntity save(UserEntity userEntity) {
        userEntity.setId(new ObjectId());
        userCollection.insertOne(userEntity);
        return userEntity;
    }

    @Override
    public List<UserEntity> findAll() {
        return userCollection.find().into(new ArrayList<>());
    }

    @Override
    public UserEntity findOne(String id) {
        return userCollection.find(eq("_id", new ObjectId(id))).first();
    }

    public UserEntity getUserId(String username) {
        return userCollection.find(eq("username", username)).first();
    }

    @Override
    public UserEntity update(UserEntity userEntity) {
        FindOneAndReplaceOptions options = new FindOneAndReplaceOptions().returnDocument(AFTER);
        return userCollection.findOneAndReplace(eq("_id", userEntity.getId()), userEntity, options);
    }
}
